using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;
using Flop.Core.Tests.Mocks;

namespace Flop.Core.Tests;

public class MeshManagerTests
{
    [Fact]
    public void GetOrCreate_UploadsMeshOnFirstCall()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);

        manager.GetOrCreate(cylinder);

        Assert.Equal(1, uploader.UploadCount);
    }

    [Fact]
    public void GetOrCreate_DoesNotUploadOnSecondCall()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);

        manager.GetOrCreate(cylinder);
        manager.GetOrCreate(cylinder);

        Assert.Equal(1, uploader.UploadCount);
    }

    [Fact]
    public void GetOrCreate_IncrementsRefCount()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var handle = new MeshHandle(cylinder);

        manager.GetOrCreate(cylinder);
        Assert.Equal(1, manager.GetRefCount(handle));

        manager.GetOrCreate(cylinder);
        Assert.Equal(2, manager.GetRefCount(handle));

        manager.GetOrCreate(cylinder);
        Assert.Equal(3, manager.GetRefCount(handle));
    }

    [Fact]
    public void Release_DecrementsRefCount()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var handle = new MeshHandle(cylinder);

        manager.GetOrCreate(cylinder);
        manager.GetOrCreate(cylinder);
        Assert.Equal(2, manager.GetRefCount(handle));

        manager.Release(handle);
        Assert.Equal(1, manager.GetRefCount(handle));
    }

    [Fact]
    public void Release_UnloadsMeshWhenRefCountReachesZero()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var handle = new MeshHandle(cylinder);

        manager.GetOrCreate(cylinder);
        manager.Release(handle);

        Assert.Equal(1, uploader.UnloadCount);
        Assert.Equal(0, manager.GetRefCount(handle));
        Assert.False(manager.IsCached(handle));
    }

    [Fact]
    public void Release_ThrowsOnInvalidHandle()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var handle = new MeshHandle(cylinder);

        var exception = Assert.Throws<InvalidOperationException>(() => manager.Release(handle));
        Assert.Contains("never acquired", exception.Message);
    }

    [Fact]
    public void Release_ThrowsOnNegativeRefCount()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var handle = new MeshHandle(cylinder);

        manager.GetOrCreate(cylinder);
        manager.Release(handle);

        var exception = Assert.Throws<InvalidOperationException>(() => manager.Release(handle));
        Assert.Contains("never acquired", exception.Message);
    }

    [Fact]
    public void ReleaseByPrimitive_WorksCorrectly()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var handle = new MeshHandle(cylinder);

        manager.GetOrCreate(cylinder);
        manager.Release(cylinder);

        Assert.Equal(1, uploader.UnloadCount);
        Assert.False(manager.IsCached(handle));
    }

    [Fact]
    public void IsCached_ReturnsTrueWhenCached()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var handle = new MeshHandle(cylinder);

        Assert.False(manager.IsCached(handle));

        manager.GetOrCreate(cylinder);
        Assert.True(manager.IsCached(handle));
    }

    [Fact]
    public void CachedMeshCount_TracksUniqueMeshes()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);

        Assert.Equal(0, manager.CachedMeshCount);

        manager.GetOrCreate(new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16));
        Assert.Equal(1, manager.CachedMeshCount);

        manager.GetOrCreate(new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16)); // Same - no increase
        Assert.Equal(1, manager.CachedMeshCount);

        manager.GetOrCreate(new Sphere(0.5f, new MaterialHandle("Test"), 16, 16)); // Different
        Assert.Equal(2, manager.CachedMeshCount);
    }

    [Fact]
    public void Dispose_UnloadsAllMeshes()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);

        manager.GetOrCreate(new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16));
        manager.GetOrCreate(new Sphere(0.5f, new MaterialHandle("Test"), 16, 16));
        manager.GetOrCreate(new Box(new Vector3(1, 2, 3), new MaterialHandle("Test")));

        manager.Dispose();

        Assert.Equal(3, uploader.UnloadCount);
        Assert.Equal(0, manager.CachedMeshCount);
    }

    [Fact]
    public void IdenticalPrimitives_ShareSameMesh()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);

        var cylinder1 = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);
        var cylinder2 = new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16);

        var mesh1 = manager.GetOrCreate(cylinder1);
        var mesh2 = manager.GetOrCreate(cylinder2);

        Assert.Equal(1, uploader.UploadCount);
        Assert.Equal(1, manager.CachedMeshCount);
    }

    [Fact]
    public void DifferentPrimitives_GetDifferentMeshes()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);

        manager.GetOrCreate(new Cylinder(0.5f, 1.0f, new MaterialHandle("Test"), 16));
        manager.GetOrCreate(new Cylinder(0.6f, 1.0f, new MaterialHandle("Test"), 16)); // Different radius

        Assert.Equal(2, uploader.UploadCount);
        Assert.Equal(2, manager.CachedMeshCount);
    }
}
