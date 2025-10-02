namespace Flop.Core.Geometry;

/// <summary>
/// A handle uniquely identifying a material for rendering.
/// Used for deduplication and render batching by material.
/// </summary>
public readonly record struct MaterialHandle(int Hash)
{
    /// <summary>
    /// Create a material handle from a material.
    /// The handle is computed from the material's properties for deterministic deduplication.
    /// </summary>
    /// <param name="material">The material to create a handle for.</param>
    public MaterialHandle(Material material)
        : this(
            HashCode.Combine(material.Color.R, material.Color.G, material.Color.B, material.Color.A)
        ) { }
}
