using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Tests.Geometry;

public class GeometryPrimitiveTests
{
    [Fact]
    public void Box_HasCorrectDimensions()
    {
        var box = new Box(new Vector3(2, 4, 6));

        Assert.Equal(2, box.SizeX);
        Assert.Equal(4, box.SizeY);
        Assert.Equal(6, box.SizeZ);
    }

    [Fact]
    public void Box_CubeFactory_CreatesUniformBox()
    {
        var cube = Box.Cube(5);

        Assert.Equal(5, cube.SizeX);
        Assert.Equal(5, cube.SizeY);
        Assert.Equal(5, cube.SizeZ);
    }

    [Fact]
    public void Box_DefaultPosition_IsZero()
    {
        var box = new Box(new Vector3(1, 1, 1));

        Assert.Equal(Vector3.Zero, box.LocalPosition);
    }

    [Fact]
    public void Box_DefaultRotation_IsIdentity()
    {
        var box = new Box(new Vector3(1, 1, 1));

        Assert.Equal(Quaternion.Identity, box.LocalRotation);
    }

    [Fact]
    public void Cylinder_HasCorrectDiameter()
    {
        var cylinder = new Cylinder(0.5f, 1.0f);

        Assert.Equal(1.0f, cylinder.Diameter);
    }

    [Fact]
    public void Sphere_HasCorrectDiameter()
    {
        var sphere = new Sphere(0.5f);

        Assert.Equal(1.0f, sphere.Diameter);
    }

    [Fact]
    public void Hemisphere_HasCorrectDiameter()
    {
        var hemisphere = new Hemisphere(0.5f);

        Assert.Equal(1.0f, hemisphere.Diameter);
    }

    [Fact]
    public void Box_BoundingBox_ReturnsSelf()
    {
        var box = new Box(new Vector3(1, 2, 3));

        Assert.Equal(box, box.BoundingBox);
    }

    [Fact]
    public void Cylinder_BoundingBox_HasCorrectDimensions()
    {
        var cylinder = new Cylinder(0.5f, 2.0f);
        var bbox = cylinder.BoundingBox;

        Assert.Equal(1.0f, bbox.SizeX); // Diameter
        Assert.Equal(2.0f, bbox.SizeY); // Height
        Assert.Equal(1.0f, bbox.SizeZ); // Diameter
    }

    [Fact]
    public void Sphere_BoundingBox_IsUniform()
    {
        var sphere = new Sphere(0.5f);
        var bbox = sphere.BoundingBox;

        Assert.Equal(1.0f, bbox.SizeX);
        Assert.Equal(1.0f, bbox.SizeY);
        Assert.Equal(1.0f, bbox.SizeZ);
    }
}
