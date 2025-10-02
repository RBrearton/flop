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
    /// Get or create a material for the given Flop material.
    /// If the material doesn't exist, it will be created and loaded to the GPU.
    /// Increments the reference count.
    /// </summary>
    public Material GetOrCreate(Flop.Core.Material material)
    {
        MaterialHandle handle = new(material);

        if (!_materialCache.TryGetValue(handle, out Material value))
        {
            var raylibMaterial = _loader.LoadMaterial(material.Color);
            value = raylibMaterial;
            _materialCache[handle] = value;
            _refCounts[handle] = 0;
        }

        _refCounts[handle]++;
        return value;
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
        Release(new MaterialHandle(material));
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
