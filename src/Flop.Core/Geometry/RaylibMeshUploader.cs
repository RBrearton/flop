using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// Raylib implementation of IMeshUploader.
/// Uploads and unloads meshes using raylib's GPU functions.
/// </summary>
public class RaylibMeshUploader : IMeshUploader
{
    public void Upload(ref Mesh mesh)
    {
        Raylib.UploadMesh(ref mesh, false);
    }

    public void Unload(Mesh mesh)
    {
        Raylib.UnloadMesh(mesh);
    }
}
