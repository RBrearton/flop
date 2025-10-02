namespace Flop.Core.Geometry;

/// <summary>
/// A handle uniquely identifying a material for rendering.
/// Used for deduplication and render batching by material.
/// </summary>
public readonly record struct MaterialHandle(int Hash)
{
    /// <summary>
    /// Create a material handle from a name.
    /// </summary>
    /// <param name="name">The unique name of the material.</param>
    public MaterialHandle(string name)
        : this(name.GetHashCode()) { }
}
