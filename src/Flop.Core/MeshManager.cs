using Flop.Core.Geometry;
using Raylib_cs;

namespace Flop.Core;

/// <summary>
/// Manages mesh caching and GPU lifecycle.
/// Ensures identical primitives share the same GPU-uploaded mesh.
/// Tracks reference counts for automatic cleanup.
/// </summary>
public class MeshManager : IDisposable
{
    private readonly Dictionary<MeshHandle, Mesh> _meshCache = [];
    private readonly Dictionary<MeshHandle, int> _refCounts = [];

    /// <summary>
    /// Get or create a mesh for the given primitive.
    /// If the mesh doesn't exist, it will be generated and uploaded to the GPU.
    /// Increments the reference count.
    /// </summary>
    public Mesh GetOrCreate(IGeometryPrimitive primitive)
    {
        MeshHandle handle = new(primitive);

        if (!_meshCache.TryGetValue(handle, out Mesh value))
        {
            var mesh = primitive.GetMesh();
            Raylib.UploadMesh(ref mesh, false);
            value = mesh;
            _meshCache[handle] = value;
            _refCounts[handle] = 0;
        }

        _refCounts[handle]++;
        return value;
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
            Raylib.UnloadMesh(_meshCache[handle]);
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
        Release(new MeshHandle(primitive));
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
            Raylib.UnloadMesh(mesh);
        }

        _meshCache.Clear();
        _refCounts.Clear();

        GC.SuppressFinalize(this);
    }
}
