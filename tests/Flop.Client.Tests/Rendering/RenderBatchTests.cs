using System.Numerics;
using Flop.Client.Rendering;
using Flop.Core;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;

namespace Flop.Client.Tests.Rendering;

public class RenderBatchTests
{
    private static readonly IGeometryPrimitive TestPrimitive = new Box(
        Vector3.One,
        Material.Default
    );
    private static readonly Material TestMaterial = Material.Default;

    [Fact]
    public void Constructor_InitializesHandles()
    {
        var meshHandle = MeshHandle.FromHashCode(MeshManager.ComputeHash(TestPrimitive));
        var materialHandle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        var batch = new RenderBatch(meshHandle, materialHandle);

        Assert.Equal(meshHandle, batch.MeshHandle);
        Assert.Equal(materialHandle, batch.MaterialHandle);
    }

    [Fact]
    public void Add_AddsTransformToBatch()
    {
        var batch = new RenderBatch(
            MeshHandle.FromHashCode(MeshManager.ComputeHash(TestPrimitive)),
            MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial))
        );
        var transform = Matrix4x4.CreateTranslation(1, 2, 3);

        batch.Add(transform);

        Assert.Single(batch.Transforms);
        Assert.Equal(transform, batch.Transforms[0]);
    }

    [Fact]
    public void Add_AccumulatesMultipleTransforms()
    {
        var batch = new RenderBatch(
            MeshHandle.FromHashCode(MeshManager.ComputeHash(TestPrimitive)),
            MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial))
        );
        var transform1 = Matrix4x4.CreateTranslation(1, 2, 3);
        var transform2 = Matrix4x4.CreateTranslation(4, 5, 6);
        var transform3 = Matrix4x4.CreateTranslation(7, 8, 9);

        batch.Add(transform1);
        batch.Add(transform2);
        batch.Add(transform3);

        Assert.Equal(3, batch.Transforms.Count);
        Assert.Equal(transform1, batch.Transforms[0]);
        Assert.Equal(transform2, batch.Transforms[1]);
        Assert.Equal(transform3, batch.Transforms[2]);
    }

    [Fact]
    public void Clear_RemovesAllTransforms()
    {
        var batch = new RenderBatch(
            MeshHandle.FromHashCode(MeshManager.ComputeHash(TestPrimitive)),
            MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial))
        );
        batch.Add(Matrix4x4.CreateTranslation(1, 2, 3));
        batch.Add(Matrix4x4.CreateTranslation(4, 5, 6));

        batch.Clear();

        Assert.Empty(batch.Transforms);
    }

    [Fact]
    public void Transforms_IsReadOnly()
    {
        var batch = new RenderBatch(
            MeshHandle.FromHashCode(MeshManager.ComputeHash(TestPrimitive)),
            MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial))
        );

        // Verify the type is IReadOnlyList
        Assert.IsAssignableFrom<IReadOnlyList<Matrix4x4>>(batch.Transforms);
    }
}
