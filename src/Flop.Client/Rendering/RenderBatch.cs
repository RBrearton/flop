using System.Numerics;
using Flop.Core.Geometry;
using Raylib_cs;

namespace Flop.Client.Rendering;

/// <summary>
/// Groups instances with the same mesh and material for efficient instanced rendering.
/// Contains transform matrices for all instances in this batch.
/// </summary>
public class RenderBatch(MeshHandle meshHandle, MaterialHandle materialHandle)
{
    private readonly List<Matrix4x4> _transforms = [];

    public MeshHandle MeshHandle { get; } = meshHandle;
    public MaterialHandle MaterialHandle { get; } = materialHandle;
    public IReadOnlyList<Matrix4x4> Transforms => _transforms;

    /// <summary>
    /// Add a transform matrix to this batch.
    /// </summary>
    public void Add(Matrix4x4 transform)
    {
        _transforms.Add(transform);
    }

    /// <summary>
    /// Clear all transforms from this batch.
    /// </summary>
    public void Clear()
    {
        _transforms.Clear();
    }

    /// <summary>
    /// Render this batch using DrawMeshInstanced.
    /// </summary>
    public unsafe void Draw(Mesh mesh, Material material)
    {
        if (_transforms.Count == 0)
            return;

        // DrawMeshInstanced expects Matrix4x4 pointer in raylib-cs
        var transformArray = _transforms.ToArray();
        fixed (Matrix4x4* transformsPtr = transformArray)
        {
            Raylib.DrawMeshInstanced(mesh, material, transformsPtr, _transforms.Count);
        }
    }
}
