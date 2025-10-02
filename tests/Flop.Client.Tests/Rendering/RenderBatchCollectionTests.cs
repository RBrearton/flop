using System.Numerics;
using Flop.Client.Rendering;
using Flop.Core.Geometry.Primitives;

namespace Flop.Client.Tests.Rendering;

public class RenderBatchCollectionTests
{
    private static readonly Flop.Core.Material TestMaterial = Flop.Core.Material.Default;

    [Fact]
    public void AddPrimitive_CreatesBatchForNewMeshMaterialPair()
    {
        var collection = new RenderBatchCollection();
        var box = new Box(new Vector3(1, 2, 3), TestMaterial);
        var transform = Matrix4x4.CreateTranslation(1, 2, 3);

        collection.AddPrimitive(box, transform);

        Assert.Equal(1, collection.BatchCount);
    }

    [Fact]
    public void AddPrimitive_ReusesBatchForSameMeshMaterialPair()
    {
        var collection = new RenderBatchCollection();
        var box1 = new Box(new Vector3(1, 2, 3), TestMaterial);
        var box2 = new Box(new Vector3(1, 2, 3), TestMaterial); // Same dimensions and material

        collection.AddPrimitive(box1, Matrix4x4.CreateTranslation(1, 0, 0));
        collection.AddPrimitive(box2, Matrix4x4.CreateTranslation(2, 0, 0));

        Assert.Equal(1, collection.BatchCount);
        Assert.Equal(2, collection.TotalInstanceCount);
    }

    [Fact]
    public void AddPrimitive_CreatesSeparateBatchesForDifferentMeshes()
    {
        var collection = new RenderBatchCollection();
        var box = new Box(new Vector3(1, 2, 3), TestMaterial);
        var sphere = new Sphere(0.5f, TestMaterial);

        collection.AddPrimitive(box, Matrix4x4.Identity);
        collection.AddPrimitive(sphere, Matrix4x4.Identity);

        Assert.Equal(2, collection.BatchCount);
    }

    [Fact]
    public void AddPrimitive_CreatesSeparateBatchesForDifferentMaterials()
    {
        var collection = new RenderBatchCollection();
        var redBox = new Box(new Vector3(1, 2, 3), new Flop.Core.Material(Flop.Core.Color.Red));
        var blueBox = new Box(new Vector3(1, 2, 3), new Flop.Core.Material(Flop.Core.Color.Blue));

        collection.AddPrimitive(redBox, Matrix4x4.Identity);
        collection.AddPrimitive(blueBox, Matrix4x4.Identity);

        Assert.Equal(2, collection.BatchCount);
    }

    [Fact]
    public void TotalInstanceCount_SumsAllInstances()
    {
        var collection = new RenderBatchCollection();
        var box = new Box(new Vector3(1, 2, 3), TestMaterial);
        var sphere = new Sphere(0.5f, TestMaterial);

        // 3 boxes
        collection.AddPrimitive(box, Matrix4x4.CreateTranslation(0, 0, 0));
        collection.AddPrimitive(box, Matrix4x4.CreateTranslation(1, 0, 0));
        collection.AddPrimitive(box, Matrix4x4.CreateTranslation(2, 0, 0));

        // 2 spheres
        collection.AddPrimitive(sphere, Matrix4x4.CreateTranslation(0, 1, 0));
        collection.AddPrimitive(sphere, Matrix4x4.CreateTranslation(1, 1, 0));

        Assert.Equal(5, collection.TotalInstanceCount);
    }

    [Fact]
    public void Clear_RemovesAllTransformsButKeepsBatches()
    {
        var collection = new RenderBatchCollection();
        var box = new Box(new Vector3(1, 2, 3), TestMaterial);

        collection.AddPrimitive(box, Matrix4x4.CreateTranslation(0, 0, 0));
        collection.AddPrimitive(box, Matrix4x4.CreateTranslation(1, 0, 0));

        collection.Clear();

        Assert.Equal(1, collection.BatchCount); // Batch still exists
        Assert.Equal(0, collection.TotalInstanceCount); // No instances
    }

    [Fact]
    public void GetBatches_ReturnsAllBatches()
    {
        var collection = new RenderBatchCollection();
        var box = new Box(new Vector3(1, 2, 3), TestMaterial);
        var sphere = new Sphere(0.5f, TestMaterial);

        collection.AddPrimitive(box, Matrix4x4.Identity);
        collection.AddPrimitive(sphere, Matrix4x4.Identity);

        var batches = collection.GetBatches().ToList();

        Assert.Equal(2, batches.Count);
    }

    [Fact]
    public void AddPrimitive_GroupsByMeshAndMaterialCorrectly()
    {
        var collection = new RenderBatchCollection();

        // Red boxes (same mesh, same material)
        var redBox1 = new Box(new Vector3(1, 1, 1), new Flop.Core.Material(Flop.Core.Color.Red));
        var redBox2 = new Box(new Vector3(1, 1, 1), new Flop.Core.Material(Flop.Core.Color.Red));

        // Blue boxes (same mesh, different material)
        var blueBox1 = new Box(new Vector3(1, 1, 1), new Flop.Core.Material(Flop.Core.Color.Blue));

        // Different size red box (different mesh, same material as red boxes)
        var bigRedBox = new Box(new Vector3(2, 2, 2), new Flop.Core.Material(Flop.Core.Color.Red));

        collection.AddPrimitive(redBox1, Matrix4x4.Identity);
        collection.AddPrimitive(redBox2, Matrix4x4.Identity);
        collection.AddPrimitive(blueBox1, Matrix4x4.Identity);
        collection.AddPrimitive(bigRedBox, Matrix4x4.Identity);

        // Should have 3 batches:
        // 1. Small red boxes (2 instances)
        // 2. Small blue box (1 instance)
        // 3. Big red box (1 instance)
        Assert.Equal(3, collection.BatchCount);
        Assert.Equal(4, collection.TotalInstanceCount);
    }
}
