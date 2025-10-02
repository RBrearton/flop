using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;
using Flop.Core.Tests.Mocks;

namespace Flop.Core.Tests;

public class MeshManagerTests
{
    [Fact]
    public void UploadMesh_UploadsMeshOnFirstCall()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);

        manager.UploadMesh(cylinder);

        Assert.Equal(1, uploader.UploadCount);
    }

    [Fact]
    public void UploadMesh_DoesNotUploadOnSecondCall()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);

        manager.UploadMesh(cylinder);
        manager.UploadMesh(cylinder);

        Assert.Equal(1, uploader.UploadCount);
    }

    [Fact]
    public void UploadMesh_IncrementsRefCount()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        manager.UploadMesh(cylinder);
        Assert.Equal(1, manager.GetRefCount(handle));

        manager.UploadMesh(cylinder);
        Assert.Equal(2, manager.GetRefCount(handle));

        manager.UploadMesh(cylinder);
        Assert.Equal(3, manager.GetRefCount(handle));
    }

    [Fact]
    public void UploadMesh_ReturnsCorrectHandle()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var expectedHandle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        var handle = manager.UploadMesh(cylinder);

        Assert.Equal(expectedHandle, handle);
    }

    [Fact]
    public void GetMesh_ReturnsMeshForUploadedHandle()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);

        var handle = manager.UploadMesh(cylinder);

        // Should not throw
        manager.GetMesh(handle);
    }

    [Fact]
    public void GetMesh_ThrowsForNonUploadedHandle()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        var exception = Assert.Throws<InvalidOperationException>(() => manager.GetMesh(handle));
        Assert.Contains("has not been uploaded", exception.Message);
    }

    [Fact]
    public void Release_DecrementsRefCount()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        manager.UploadMesh(cylinder);
        manager.UploadMesh(cylinder);
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
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        manager.UploadMesh(cylinder);
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
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        var exception = Assert.Throws<InvalidOperationException>(() => manager.Release(handle));
        Assert.Contains("never acquired", exception.Message);
    }

    [Fact]
    public void Release_ThrowsOnNegativeRefCount()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        manager.UploadMesh(cylinder);
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
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        manager.UploadMesh(cylinder);
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
        var cylinder = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var handle = MeshHandle.FromHashCode(MeshManager.ComputeHash(cylinder));

        Assert.False(manager.IsCached(handle));

        manager.UploadMesh(cylinder);
        Assert.True(manager.IsCached(handle));
    }

    [Fact]
    public void CachedMeshCount_TracksUniqueMeshes()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);

        Assert.Equal(0, manager.CachedMeshCount);

        manager.UploadMesh(new Cylinder(0.5f, 1.0f, Material.Default, 16));
        Assert.Equal(1, manager.CachedMeshCount);

        manager.UploadMesh(new Cylinder(0.5f, 1.0f, Material.Default, 16)); // Same - no increase
        Assert.Equal(1, manager.CachedMeshCount);

        manager.UploadMesh(new Sphere(0.5f, Material.Default, 16, 16)); // Different
        Assert.Equal(2, manager.CachedMeshCount);
    }

    [Fact]
    public void Dispose_UnloadsAllMeshes()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);

        manager.UploadMesh(new Cylinder(0.5f, 1.0f, Material.Default, 16));
        manager.UploadMesh(new Sphere(0.5f, Material.Default, 16, 16));
        manager.UploadMesh(new Box(new Vector3(1, 2, 3), Material.Default));

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

        var cylinder1 = new Cylinder(0.5f, 1.0f, Material.Default, 16);
        var cylinder2 = new Cylinder(0.5f, 1.0f, Material.Default, 16);

        manager.UploadMesh(cylinder1);
        manager.UploadMesh(cylinder2);

        Assert.Equal(1, uploader.UploadCount);
        Assert.Equal(1, manager.CachedMeshCount);
    }

    [Fact]
    public void DifferentPrimitives_GetDifferentMeshes()
    {
        var uploader = new MockMeshUploader();
        var generator = new MockMeshGenerator();
        var manager = new MeshManager(uploader, generator);

        manager.UploadMesh(new Cylinder(0.5f, 1.0f, Material.Default, 16));
        manager.UploadMesh(new Cylinder(0.6f, 1.0f, Material.Default, 16)); // Different radius

        Assert.Equal(2, uploader.UploadCount);
        Assert.Equal(2, manager.CachedMeshCount);
    }
}
