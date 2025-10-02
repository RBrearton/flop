namespace Flop.Core.Geometry;

/// <summary>
/// A handle uniquely identifying a material for rendering.
/// Used for deduplication and render batching by material.
/// Handles should be created via MaterialManager.UploadMaterial().
/// </summary>
public readonly record struct MaterialHandle
{
    private MaterialHandle(int hash)
    {
        Hash = hash;
    }

    public int Hash { get; }

    /// <summary>
    /// Create a MaterialHandle from a raw hash code.
    /// This is primarily for deserialization, testing, or advanced scenarios.
    /// In normal usage, obtain handles via MaterialManager.UploadMaterial().
    /// </summary>
    public static MaterialHandle FromHashCode(int hash) => new(hash);
}
