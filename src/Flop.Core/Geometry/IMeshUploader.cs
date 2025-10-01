using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// Interface for uploading and unloading meshes to/from GPU.
/// Abstracts raylib calls for testability.
/// </summary>
public interface IMeshUploader
{
    /// <summary>
    /// Upload a mesh to the GPU.
    /// </summary>
    void Upload(ref Mesh mesh);

    /// <summary>
    /// Unload a mesh from the GPU.
    /// </summary>
    void Unload(Mesh mesh);
}
