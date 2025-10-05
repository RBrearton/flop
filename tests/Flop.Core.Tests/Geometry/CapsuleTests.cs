using Flop.Core.Geometry;
using Flop.Core.Geometry.Components;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Tests.Geometry;

public class CapsuleTests
{
    private static readonly Material TestMaterial = Material.Default;

    [Fact]
    public void Capsule_HasCorrectTotalHeight()
    {
        var capsule = new Capsule(radius: 0.5f, height: 2.0f, TestMaterial);

        // Total height = cylinder height + 2 * radius (caps)
        Assert.Equal(3.0f, capsule.TotalHeight);
    }

    [Fact]
    public void Capsule_HasThreePrimitives()
    {
        var capsule = new Capsule(0.5f, 2.0f, TestMaterial);

        Assert.Equal(3, capsule.Primitives.Count);
    }

    [Fact]
    public void Capsule_FirstPrimitive_IsCylinder()
    {
        var capsule = new Capsule(0.5f, 2.0f, TestMaterial);

        Assert.IsType<Cylinder>(capsule.Primitives[0]);
    }

    [Fact]
    public void Capsule_HasTwoHemispheres()
    {
        var capsule = new Capsule(0.5f, 2.0f, TestMaterial);

        var hemisphereCount = capsule.Primitives.Count(p => p is Hemisphere);
        Assert.Equal(2, hemisphereCount);
    }

    [Fact]
    public void Capsule_BoundingBox_UsesTotalHeight()
    {
        var capsule = new Capsule(radius: 0.5f, height: 2.0f, TestMaterial);
        var bbox = capsule.BoundingBox;

        Assert.Equal(1.0f, bbox.Size.X, precision: 2); // Diameter
        Assert.Equal(3.0f, bbox.Size.Y, precision: 2); // TotalHeight
        Assert.Equal(1.0f, bbox.Size.Z, precision: 2); // Diameter
    }

    [Fact]
    public void Capsule_Hemispheres_HaveCorrectRadius()
    {
        var capsule = new Capsule(radius: 0.5f, height: 2.0f, TestMaterial);

        var hemispheres = capsule.Primitives.OfType<Hemisphere>().ToList();

        Assert.All(hemispheres, h => Assert.Equal(0.5f, h.Radius));
    }

    [Fact]
    public void Capsule_CylinderBody_HasCorrectDimensions()
    {
        var capsule = new Capsule(radius: 0.5f, height: 2.0f, TestMaterial, slices: 16);
        var cylinder = capsule.Primitives.OfType<Cylinder>().First();

        Assert.Equal(0.5f, cylinder.Radius);
        Assert.Equal(2.0f, cylinder.Height);
        Assert.Equal(16, cylinder.Slices);
    }
}
