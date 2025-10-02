namespace Flop.Core.Geometry;

/// <summary>
/// A handle that uniquely identifies a mesh based on its primitive parameters.
/// Two primitives with identical parameters will produce the same handle,
/// enabling mesh deduplication and GPU caching.
/// Handles should be created via MeshManager.UploadMesh().
/// </summary>
public readonly record struct MeshHandle
{
    private MeshHandle(int hash)
    {
        Hash = hash;
    }

    public int Hash { get; }

    /// <summary>
    /// Create a MeshHandle from a raw hash code.
    /// This is primarily for deserialization, testing, or advanced scenarios.
    /// In normal usage, obtain handles via MeshManager.UploadMesh().
    /// </summary>
    public static MeshHandle FromHashCode(int hash) => new(hash);
}
