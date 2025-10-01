using Flop.Core.Geometry;
using Raylib_cs;

namespace Flop.Core.Tests.Mocks;

/// <summary>
/// Mock implementation of IMeshUploader for testing.
/// Tracks upload/unload calls without actually touching the GPU.
/// </summary>
public class MockMeshUploader : IMeshUploader
{
    public List<Mesh> UploadedMeshes { get; } = [];
    public List<Mesh> UnloadedMeshes { get; } = [];

    public int UploadCount => UploadedMeshes.Count;
    public int UnloadCount => UnloadedMeshes.Count;

    public void Upload(ref Mesh mesh)
    {
        UploadedMeshes.Add(mesh);
    }

    public void Unload(Mesh mesh)
    {
        UnloadedMeshes.Add(mesh);
    }

    public void Clear()
    {
        UploadedMeshes.Clear();
        UnloadedMeshes.Clear();
    }
}
