using Flop.Core.Geometry;
using Raylib_cs;

namespace Flop.Client.Rendering;

/// <summary>
/// Manages material caching and GPU lifecycle.
/// Ensures identical materials share the same GPU-loaded material.
/// Tracks reference counts for automatic cleanup.
/// </summary>
public class MaterialManager(IMaterialLoader loader) : IDisposable
{
    private readonly Dictionary<MaterialHandle, Material> _materialCache = [];
    private readonly Dictionary<MaterialHandle, int> _refCounts = [];
    private readonly IMaterialLoader _loader = loader;

    public MaterialManager()
        : this(new RaylibMaterialLoader()) { }

    /// <summary>
    /// Upload a material to the GPU.
    /// If the material doesn't exist, it will be created and loaded to the GPU.
    /// Increments the reference count.
    /// Returns the handle that can be used to retrieve the material.
    /// </summary>
    public MaterialHandle UploadMaterial(Flop.Core.Material material)
    {
        var hash = ComputeHash(material);
        MaterialHandle handle = MaterialHandle.FromHashCode(hash);

        if (!_materialCache.TryGetValue(handle, out Material _))
        {
            var raylibMaterial = _loader.LoadMaterial(material.Color);
            _materialCache[handle] = raylibMaterial;
            _refCounts[handle] = 0;
        }

        _refCounts[handle]++;
        return handle;
    }

    /// <summary>
    /// Compute a deterministic hash for a material based on its properties.
    /// Identical materials will produce the same hash, enabling deduplication.
    /// </summary>
    public static int ComputeHash(Flop.Core.Material material)
    {
        return HashCode.Combine(
            material.Color.R,
            material.Color.G,
            material.Color.B,
            material.Color.A
        );
    }

    /// <summary>
    /// Get the GPU material corresponding to the given handle.
    /// Throws if the handle has not been uploaded.
    /// </summary>
    public Material GetMaterial(MaterialHandle handle)
    {
        if (!_materialCache.TryGetValue(handle, out Material material))
        {
            throw new InvalidOperationException(
                $"Material handle {handle} has not been uploaded. Call UploadMaterial first."
            );
        }

        return material;
    }

    /// <summary>
    /// Release a reference to a material.
    /// When reference count reaches zero, the material is unloaded from GPU and removed from cache.
    /// </summary>
    public void Release(MaterialHandle handle)
    {
        if (!_refCounts.TryGetValue(handle, out int value))
        {
            throw new InvalidOperationException(
                $"Attempted to release material handle that was never acquired: {handle}"
            );
        }

        _refCounts[handle] = --value;

        if (value == 0)
        {
            _loader.UnloadMaterial(_materialCache[handle]);
            _materialCache.Remove(handle);
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
    /// Release a reference to a material by Flop material.
    /// Convenience method that computes the handle from the material.
    /// </summary>
    public void Release(Flop.Core.Material material)
    {
        var hash = ComputeHash(material);
        Release(MaterialHandle.FromHashCode(hash));
    }

    /// <summary>
    /// Get the current reference count for a material handle.
    /// Returns 0 if the handle is not in the cache.
    /// </summary>
    public int GetRefCount(MaterialHandle handle)
    {
        return _refCounts.GetValueOrDefault(handle, 0);
    }

    /// <summary>
    /// Check if a material is currently cached.
    /// </summary>
    public bool IsCached(MaterialHandle handle)
    {
        return _materialCache.ContainsKey(handle);
    }

    /// <summary>
    /// Get the total number of unique materials currently cached.
    /// </summary>
    public int CachedMaterialCount => _materialCache.Count;

    /// <summary>
    /// Cleanup all materials and reset the manager.
    /// Should be called on shutdown.
    /// </summary>
    public void Dispose()
    {
        foreach (var material in _materialCache.Values)
        {
            _loader.UnloadMaterial(material);
        }

        _materialCache.Clear();
        _refCounts.Clear();

        GC.SuppressFinalize(this);
    }
}
