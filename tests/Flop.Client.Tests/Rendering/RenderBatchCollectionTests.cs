using System.Numerics;
using Flop.Client.Rendering;
using Flop.Core;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;

namespace Flop.Client.Tests.Rendering;

public class RenderBatchCollectionTests
{
    private static readonly Flop.Core.Material TestMaterial = Flop.Core.Material.Default;

    [Fact]
    public void AddInstance_CreatesBatchForNewMeshMaterialPair()
    {
        var collection = new RenderBatchCollection();
        var boxHandle = MeshHandle.FromHashCode(1);
        var materialHandle = MaterialHandle.FromHashCode(1);

        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.Identity);

        Assert.Equal(1, collection.BatchCount);
    }

    [Fact]
    public void AddInstance_ReusesBatchForSameMeshMaterialPair()
    {
        var collection = new RenderBatchCollection();
        var box1Handle = MeshHandle.FromHashCode(1);
        var box2Handle = MeshHandle.FromHashCode(1); // Same hash as box1
        var materialHandle = MaterialHandle.FromHashCode(1);

        collection.AddInstance(box1Handle, materialHandle, Matrix4x4.CreateTranslation(1, 0, 0));
        collection.AddInstance(box2Handle, materialHandle, Matrix4x4.CreateTranslation(2, 0, 0));

        Assert.Equal(1, collection.BatchCount);
        Assert.Equal(2, collection.TotalInstanceCount);
    }

    [Fact]
    public void AddInstance_CreatesSeparateBatchesForDifferentMeshes()
    {
        var collection = new RenderBatchCollection();
        var boxHandle = MeshHandle.FromHashCode(1);
        var sphereHandle = MeshHandle.FromHashCode(2);
        var materialHandle = MaterialHandle.FromHashCode(1);

        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.Identity);
        collection.AddInstance(sphereHandle, materialHandle, Matrix4x4.Identity);

        Assert.Equal(2, collection.BatchCount);
    }

    [Fact]
    public void AddInstance_CreatesSeparateBatchesForDifferentMaterials()
    {
        var collection = new RenderBatchCollection();
        var boxHandle = MeshHandle.FromHashCode(1);
        var redMaterialHandle = MaterialHandle.FromHashCode(1);
        var blueMaterialHandle = MaterialHandle.FromHashCode(2);

        collection.AddInstance(boxHandle, redMaterialHandle, Matrix4x4.Identity);
        collection.AddInstance(boxHandle, blueMaterialHandle, Matrix4x4.Identity);

        Assert.Equal(2, collection.BatchCount);
    }

    [Fact]
    public void TotalInstanceCount_SumsAllInstances()
    {
        var collection = new RenderBatchCollection();
        var boxHandle = MeshHandle.FromHashCode(1);
        var sphereHandle = MeshHandle.FromHashCode(2);
        var materialHandle = MaterialHandle.FromHashCode(1);

        // 3 boxes
        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.CreateTranslation(0, 0, 0));
        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.CreateTranslation(1, 0, 0));
        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.CreateTranslation(2, 0, 0));

        // 2 spheres
        collection.AddInstance(sphereHandle, materialHandle, Matrix4x4.CreateTranslation(0, 1, 0));
        collection.AddInstance(sphereHandle, materialHandle, Matrix4x4.CreateTranslation(1, 1, 0));

        Assert.Equal(5, collection.TotalInstanceCount);
        Assert.Equal(2, collection.BatchCount);
    }

    [Fact]
    public void Clear_RemovesAllTransformsButKeepsBatches()
    {
        var collection = new RenderBatchCollection();
        var boxHandle = MeshHandle.FromHashCode(1);
        var materialHandle = MaterialHandle.FromHashCode(1);

        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.CreateTranslation(0, 0, 0));
        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.CreateTranslation(1, 0, 0));

        collection.Clear();

        Assert.Equal(1, collection.BatchCount); // Batch still exists
        Assert.Equal(0, collection.TotalInstanceCount); // No instances
    }

    [Fact]
    public void GetBatches_ReturnsAllBatches()
    {
        var collection = new RenderBatchCollection();
        var boxHandle = MeshHandle.FromHashCode(1);
        var materialHandle = MaterialHandle.FromHashCode(1);
        var sphereHandle = MeshHandle.FromHashCode(2);

        collection.AddInstance(boxHandle, materialHandle, Matrix4x4.Identity);
        collection.AddInstance(sphereHandle, materialHandle, Matrix4x4.Identity);

        var batches = collection.GetBatches().ToList();

        Assert.Equal(2, batches.Count);
    }

    [Fact]
    public void AddInstance_GroupsByMeshAndMaterialCorrectly()
    {
        var collection = new RenderBatchCollection();

        // Red boxes (same mesh, same material)
        var box_handle = MeshHandle.FromHashCode(1);
        var big_box_handle = MeshHandle.FromHashCode(2);
        var red_material_handle = MaterialHandle.FromHashCode(1);
        var blue_material_handle = MaterialHandle.FromHashCode(2);

        collection.AddInstance(box_handle, red_material_handle, Matrix4x4.Identity);
        collection.AddInstance(box_handle, red_material_handle, Matrix4x4.Identity);
        collection.AddInstance(box_handle, blue_material_handle, Matrix4x4.Identity);
        collection.AddInstance(big_box_handle, red_material_handle, Matrix4x4.Identity);

        // Should have 3 batches:
        // 1. Small red boxes (2 instances)
        // 2. Small blue box (1 instance)
        // 3. Big red box (1 instance)
        Assert.Equal(3, collection.BatchCount);
        Assert.Equal(4, collection.TotalInstanceCount);
    }
}
