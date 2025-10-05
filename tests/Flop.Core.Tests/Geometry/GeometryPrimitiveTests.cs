using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Tests.Geometry;

public class GeometryPrimitiveTests
{
    private static readonly Material TestMaterial = Material.Default;

    [Fact]
    public void Box_HasCorrectDimensions()
    {
        var box = new Box(new Vector3(2, 4, 6), TestMaterial);

        Assert.Equal(2, box.SizeX);
        Assert.Equal(4, box.SizeY);
        Assert.Equal(6, box.SizeZ);
    }

    [Fact]
    public void Box_CubeFactory_CreatesUniformBox()
    {
        var cube = Box.Cube(5, TestMaterial);

        Assert.Equal(5, cube.SizeX);
        Assert.Equal(5, cube.SizeY);
        Assert.Equal(5, cube.SizeZ);
    }

    [Fact]
    public void Box_DefaultPosition_IsZero()
    {
        var box = new Box(new Vector3(1, 1, 1), TestMaterial);

        Assert.Equal(Vector3.Zero, box.LocalPosition);
    }

    [Fact]
    public void Box_DefaultRotation_IsIdentity()
    {
        var box = new Box(new Vector3(1, 1, 1), TestMaterial);

        Assert.Equal(Quaternion.Identity, box.LocalRotation);
    }

    [Fact]
    public void Cylinder_HasCorrectDiameter()
    {
        var cylinder = new Cylinder(0.5f, 1.0f, TestMaterial);

        Assert.Equal(1.0f, cylinder.Diameter);
    }

    [Fact]
    public void Sphere_HasCorrectDiameter()
    {
        var sphere = new Sphere(0.5f, TestMaterial);

        Assert.Equal(1.0f, sphere.Diameter);
    }

    [Fact]
    public void Hemisphere_HasCorrectDiameter()
    {
        var hemisphere = new Hemisphere(0.5f, TestMaterial);

        Assert.Equal(1.0f, hemisphere.Diameter);
    }

    [Fact]
    public void Box_BoundingBox_HasCorrectCenterAndSize()
    {
        var box = new Box(new Vector3(1, 2, 3), TestMaterial, new Vector3(5, 5, 5));
        var bbox = box.BoundingBox;

        Assert.Equal(new Vector3(5, 5, 5), bbox.Center);
        Assert.Equal(new Vector3(1, 2, 3), bbox.Size);
    }

    [Fact]
    public void Cylinder_BoundingBox_HasCorrectDimensions()
    {
        var cylinder = new Cylinder(0.5f, 2.0f, TestMaterial);
        var bbox = cylinder.BoundingBox;

        Assert.Equal(1.0f, bbox.Size.X, precision: 2); // Diameter
        Assert.Equal(2.0f, bbox.Size.Y, precision: 2); // Height
        Assert.Equal(1.0f, bbox.Size.Z, precision: 2); // Diameter
    }

    [Fact]
    public void Sphere_BoundingBox_IsUniform()
    {
        var sphere = new Sphere(0.5f, TestMaterial);
        var bbox = sphere.BoundingBox;

        Assert.Equal(1.0f, bbox.Size.X, precision: 2);
        Assert.Equal(1.0f, bbox.Size.Y, precision: 2);
        Assert.Equal(1.0f, bbox.Size.Z, precision: 2);
    }

    [Fact]
    public void Box_BoundingBox_ExpandsWhenRotated()
    {
        // Create a box rotated 45 degrees around Y-axis
        var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 4f);
        var box = new Box(2, 1, 1, TestMaterial, Vector3.Zero, rotation);
        var bbox = box.BoundingBox;

        // AABB should be larger than original box in X and Z
        Assert.True(bbox.Size.X > 2.0f);
        Assert.True(bbox.Size.Z > 1.0f);
        Assert.Equal(1.0f, bbox.Size.Y, precision: 3); // Y unchanged
    }

    [Fact]
    public void Cylinder_BoundingBox_ExpandsWhenRotatedToHorizontal()
    {
        // Rotate cylinder 90 degrees to lie on X-axis
        var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI / 2f);
        var cylinder = new Cylinder(0.5f, 2.0f, TestMaterial, localRotation: rotation);
        var bbox = cylinder.BoundingBox;

        // Height (2) should now be along X-axis
        Assert.Equal(2.0f, bbox.Size.X, precision: 2);
        Assert.Equal(1.0f, bbox.Size.Y, precision: 2); // Diameter
        Assert.Equal(1.0f, bbox.Size.Z, precision: 2); // Diameter
    }

    [Fact]
    public void Box_Center_EqualsLocalPosition()
    {
        var position = new Vector3(3, 4, 5);
        var box = new Box(1, 2, 3, TestMaterial, position);

        Assert.Equal(position, box.Center);
    }

    [Fact]
    public void Sphere_RotationDoesNotAffectBoundingBox()
    {
        var rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 3f);
        var sphere1 = new Sphere(1.0f, TestMaterial);
        var sphere2 = new Sphere(1.0f, TestMaterial, localRotation: rotation);

        Assert.Equal(sphere1.BoundingBox.Size, sphere2.BoundingBox.Size);
    }
}
