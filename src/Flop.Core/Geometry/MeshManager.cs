using Flop.Core.Geometry.Primitives;
using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// Manages mesh caching and GPU lifecycle.
/// Ensures identical primitives share the same GPU-uploaded mesh.
/// Tracks reference counts for automatic cleanup.
/// </summary>
public class MeshManager(IMeshUploader uploader, IMeshGenerator generator) : IDisposable
{
    private readonly Dictionary<MeshHandle, Mesh> _meshCache = [];
    private readonly Dictionary<MeshHandle, int> _refCounts = [];
    private readonly IMeshUploader _uploader = uploader;
    private readonly IMeshGenerator _generator = generator;

    public MeshManager()
        : this(new RaylibMeshUploader(), new RaylibMeshGenerator()) { }

    /// <summary>
    /// Upload a mesh for the given primitive to the GPU.
    /// If the mesh doesn't exist, it will be generated and uploaded to the GPU.
    /// Increments the reference count.
    /// Returns the handle that can be used to retrieve the mesh.
    /// </summary>
    public MeshHandle UploadMesh(IGeometryPrimitive primitive)
    {
        var hash = ComputeHash(primitive);
        MeshHandle handle = MeshHandle.FromHashCode(hash);

        if (!_meshCache.TryGetValue(handle, out Mesh _))
        {
            var mesh = primitive.GetMesh(_generator);
            _uploader.Upload(ref mesh);
            _meshCache[handle] = mesh;
            _refCounts[handle] = 0;
        }

        _refCounts[handle]++;
        return handle;
    }

    /// <summary>
    /// Compute a deterministic hash for a primitive based on its geometry parameters.
    /// Identical primitives will produce the same hash, enabling deduplication.
    /// </summary>
    public static int ComputeHash(IGeometryPrimitive primitive)
    {
        return primitive switch
        {
            Box box => HashCode.Combine(nameof(Box), box.Size.X, box.Size.Y, box.Size.Z),
            Cylinder cylinder
                => HashCode.Combine(
                    nameof(Cylinder),
                    cylinder.Radius,
                    cylinder.Height,
                    cylinder.Slices
                ),
            Sphere sphere
                => HashCode.Combine(nameof(Sphere), sphere.Radius, sphere.Rings, sphere.Slices),
            Hemisphere hemisphere
                => HashCode.Combine(
                    nameof(Hemisphere),
                    hemisphere.Radius,
                    hemisphere.Rings,
                    hemisphere.Slices
                ),
            _ => throw new ArgumentException($"Unknown primitive type: {primitive.GetType().Name}"),
        };
    }

    /// <summary>
    /// Get the GPU mesh corresponding to the given handle.
    /// Throws if the handle has not been uploaded.
    /// </summary>
    public Mesh GetMesh(MeshHandle handle)
    {
        if (!_meshCache.TryGetValue(handle, out Mesh mesh))
        {
            throw new InvalidOperationException(
                $"Mesh handle {handle} has not been uploaded. Call UploadMesh first."
            );
        }

        return mesh;
    }

    /// <summary>
    /// Release a reference to a mesh.
    /// When reference count reaches zero, the mesh is unloaded from GPU and removed from cache.
    /// </summary>
    public void Release(MeshHandle handle)
    {
        if (!_refCounts.TryGetValue(handle, out int value))
        {
            throw new InvalidOperationException(
                $"Attempted to release mesh handle that was never acquired: {handle}"
            );
        }

        _refCounts[handle] = --value;

        if (value == 0)
        {
            _uploader.Unload(_meshCache[handle]);
            _meshCache.Remove(handle);
            _refCounts.Remove(handle);
        }
        else if (_refCounts[handle] < 0)
        {
            throw new InvalidOperationException(
                $"Reference count went negative for handle {handle}. This indicates a bug "
                    + "in release tracking."
            );
        }
    }

    /// <summary>
    /// Release a reference to a mesh by primitive.
    /// Convenience method that computes the handle from the primitive.
    /// </summary>
    public void Release(IGeometryPrimitive primitive)
    {
        var hash = ComputeHash(primitive);
        Release(MeshHandle.FromHashCode(hash));
    }

    /// <summary>
    /// Get the current reference count for a mesh handle.
    /// Returns 0 if the handle is not in the cache.
    /// </summary>
    public int GetRefCount(MeshHandle handle)
    {
        return _refCounts.GetValueOrDefault(handle, 0);
    }

    /// <summary>
    /// Check if a mesh is currently cached.
    /// </summary>
    public bool IsCached(MeshHandle handle)
    {
        return _meshCache.ContainsKey(handle);
    }

    /// <summary>
    /// Get the total number of unique meshes currently cached.
    /// </summary>
    public int CachedMeshCount => _meshCache.Count;

    /// <summary>
    /// Cleanup all meshes and reset the manager.
    /// Should be called on shutdown.
    /// </summary>
    public void Dispose()
    {
        foreach (var mesh in _meshCache.Values)
        {
            _uploader.Unload(mesh);
        }

        _meshCache.Clear();
        _refCounts.Clear();

        GC.SuppressFinalize(this);
    }
}
