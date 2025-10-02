using Flop.Client.Rendering;
using Flop.Client.Tests.Mocks;
using Flop.Core;
using Flop.Core.Geometry;

namespace Flop.Client.Tests.Rendering;

public class MaterialManagerTests
{
    private static readonly Material TestMaterial = Material.Default;

    [Fact]
    public void GetOrCreate_LoadsMaterialOnFirstCall()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.GetOrCreate(TestMaterial);

        Assert.Equal(1, loader.LoadCount);
    }

    [Fact]
    public void GetOrCreate_DoesNotLoadOnSecondCall()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.GetOrCreate(TestMaterial);
        manager.GetOrCreate(TestMaterial);

        Assert.Equal(1, loader.LoadCount);
    }

    [Fact]
    public void GetOrCreate_IncrementsRefCount()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = new MaterialHandle(TestMaterial);

        manager.GetOrCreate(TestMaterial);
        Assert.Equal(1, manager.GetRefCount(handle));

        manager.GetOrCreate(TestMaterial);
        Assert.Equal(2, manager.GetRefCount(handle));

        manager.GetOrCreate(TestMaterial);
        Assert.Equal(3, manager.GetRefCount(handle));
    }

    [Fact]
    public void Release_DecrementsRefCount()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = new MaterialHandle(TestMaterial);

        manager.GetOrCreate(TestMaterial);
        manager.GetOrCreate(TestMaterial);
        Assert.Equal(2, manager.GetRefCount(handle));

        manager.Release(handle);
        Assert.Equal(1, manager.GetRefCount(handle));
    }

    [Fact]
    public void Release_UnloadsMaterialWhenRefCountReachesZero()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = new MaterialHandle(TestMaterial);

        manager.GetOrCreate(TestMaterial);
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
        var handle = new MaterialHandle(TestMaterial);

        var exception = Assert.Throws<InvalidOperationException>(() => manager.Release(handle));
        Assert.Contains("never acquired", exception.Message);
    }

    [Fact]
    public void Release_ThrowsOnNegativeRefCount()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = new MaterialHandle(TestMaterial);

        manager.GetOrCreate(TestMaterial);
        manager.Release(handle);

        var exception = Assert.Throws<InvalidOperationException>(() => manager.Release(handle));
        Assert.Contains("never acquired", exception.Message);
    }

    [Fact]
    public void ReleaseByMaterial_WorksCorrectly()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = new MaterialHandle(TestMaterial);

        manager.GetOrCreate(TestMaterial);
        manager.Release(TestMaterial);

        Assert.Equal(1, loader.UnloadCount);
        Assert.False(manager.IsCached(handle));
    }

    [Fact]
    public void IsCached_ReturnsTrueWhenCached()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);
        var handle = new MaterialHandle(TestMaterial);

        Assert.False(manager.IsCached(handle));

        manager.GetOrCreate(TestMaterial);
        Assert.True(manager.IsCached(handle));
    }

    [Fact]
    public void CachedMaterialCount_TracksUniqueMaterials()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        Assert.Equal(0, manager.CachedMaterialCount);

        manager.GetOrCreate(TestMaterial);
        Assert.Equal(1, manager.CachedMaterialCount);

        manager.GetOrCreate(TestMaterial); // Same - no increase
        Assert.Equal(1, manager.CachedMaterialCount);

        var redMaterial = new Material(Color.Red);
        manager.GetOrCreate(redMaterial); // Different
        Assert.Equal(2, manager.CachedMaterialCount);
    }

    [Fact]
    public void Dispose_UnloadsAllMaterials()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.GetOrCreate(TestMaterial);
        manager.GetOrCreate(new Material(Color.Red));
        manager.GetOrCreate(new Material(Color.Green));

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

        manager.GetOrCreate(material1);
        manager.GetOrCreate(material2);

        Assert.Equal(1, loader.LoadCount);
        Assert.Equal(1, manager.CachedMaterialCount);
    }

    [Fact]
    public void DifferentMaterials_GetDifferentRaylibMaterials()
    {
        var loader = new MockMaterialLoader();
        var manager = new MaterialManager(loader);

        manager.GetOrCreate(new Material(Color.Red));
        manager.GetOrCreate(new Material(Color.Blue));

        Assert.Equal(2, loader.LoadCount);
        Assert.Equal(2, manager.CachedMaterialCount);
    }
}
