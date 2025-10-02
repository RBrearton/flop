using Flop.Client.Rendering;
using Flop.Client.Tests.Mocks;
using Flop.Core;
using Flop.Core.Geometry;

namespace Flop.Client.Tests.Rendering;

public class MaterialManagerTests
{
    private static readonly Material TestMaterial = Material.Default;

    [Fact]
    public void UploadMaterial_LoadsMaterialOnFirstCall()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.UploadMaterial(TestMaterial);

        Assert.Equal(1, loader.LoadCount);
    }

    [Fact]
    public void UploadMaterial_DoesNotLoadOnSecondCall()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.UploadMaterial(TestMaterial);
        manager.UploadMaterial(TestMaterial);

        Assert.Equal(1, loader.LoadCount);
    }

    [Fact]
    public void UploadMaterial_IncrementsRefCount()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        manager.UploadMaterial(TestMaterial);
        Assert.Equal(1, manager.GetRefCount(handle));

        manager.UploadMaterial(TestMaterial);
        Assert.Equal(2, manager.GetRefCount(handle));

        manager.UploadMaterial(TestMaterial);
        Assert.Equal(3, manager.GetRefCount(handle));
    }

    [Fact]
    public void UploadMaterial_ReturnsCorrectHandle()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var expectedHandle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        var handle = manager.UploadMaterial(TestMaterial);

        Assert.Equal(expectedHandle, handle);
    }

    [Fact]
    public void GetMaterial_ReturnsMaterialForUploadedHandle()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        var handle = manager.UploadMaterial(TestMaterial);

        // Should not throw
        manager.GetMaterial(handle);
    }

    [Fact]
    public void GetMaterial_ThrowsForNonUploadedHandle()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        var exception = Assert.Throws<InvalidOperationException>(
            () => manager.GetMaterial(handle)
        );
        Assert.Contains("has not been uploaded", exception.Message);
    }

    [Fact]
    public void Release_DecrementsRefCount()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        manager.UploadMaterial(TestMaterial);
        manager.UploadMaterial(TestMaterial);
        Assert.Equal(2, manager.GetRefCount(handle));

        manager.Release(handle);
        Assert.Equal(1, manager.GetRefCount(handle));
    }

    [Fact]
    public void Release_UnloadsMaterialWhenRefCountReachesZero()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        manager.UploadMaterial(TestMaterial);
        manager.Release(handle);

        Assert.Equal(1, loader.UnloadCount);
        Assert.Equal(0, manager.GetRefCount(handle));
        Assert.False(manager.IsCached(handle));
    }

    [Fact]
    public void Release_ThrowsOnInvalidHandle()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        var exception = Assert.Throws<InvalidOperationException>(() => manager.Release(handle));
        Assert.Contains("never acquired", exception.Message);
    }

    [Fact]
    public void Release_ThrowsOnNegativeRefCount()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        manager.UploadMaterial(TestMaterial);
        manager.Release(handle);

        var exception = Assert.Throws<InvalidOperationException>(() => manager.Release(handle));
        Assert.Contains("never acquired", exception.Message);
    }

    [Fact]
    public void ReleaseByMaterial_WorksCorrectly()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        manager.UploadMaterial(TestMaterial);
        manager.Release(TestMaterial);

        Assert.Equal(1, loader.UnloadCount);
        Assert.False(manager.IsCached(handle));
    }

    [Fact]
    public void IsCached_ReturnsTrueWhenCached()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = MaterialHandle.FromHashCode(MaterialManager.ComputeHash(TestMaterial));

        Assert.False(manager.IsCached(handle));

        manager.UploadMaterial(TestMaterial);
        Assert.True(manager.IsCached(handle));
    }

    [Fact]
    public void CachedMaterialCount_TracksUniqueMaterials()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        Assert.Equal(0, manager.CachedMaterialCount);

        manager.UploadMaterial(TestMaterial);
        Assert.Equal(1, manager.CachedMaterialCount);

        manager.UploadMaterial(TestMaterial); // Same - no increase
        Assert.Equal(1, manager.CachedMaterialCount);

        var redMaterial = new Material(Color.Red);
        manager.UploadMaterial(redMaterial); // Different
        Assert.Equal(2, manager.CachedMaterialCount);
    }

    [Fact]
    public void Dispose_UnloadsAllMaterials()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.UploadMaterial(TestMaterial);
        manager.UploadMaterial(new Material(Color.Red));
        manager.UploadMaterial(new Material(Color.Green));

        manager.Dispose();

        Assert.Equal(3, loader.UnloadCount);
        Assert.Equal(0, manager.CachedMaterialCount);
    }

    [Fact]
    public void IdenticalMaterials_ShareSameRaylibMaterial()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        var material1 = new Material(Color.Red);
        var material2 = new Material(Color.Red);

        manager.UploadMaterial(material1);
        manager.UploadMaterial(material2);

        Assert.Equal(1, loader.LoadCount);
        Assert.Equal(1, manager.CachedMaterialCount);
    }

    [Fact]
    public void DifferentMaterials_GetDifferentRaylibMaterials()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.UploadMaterial(new Material(Color.Red));
        manager.UploadMaterial(new Material(Color.Blue));

        Assert.Equal(2, loader.LoadCount);
        Assert.Equal(2, manager.CachedMaterialCount);
    }
}
