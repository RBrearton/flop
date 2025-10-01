using System.Numerics;
using Flop.Core.Geometry;

namespace Flop.Core.Tests.Geometry;

public class MeshHandleTests
{
    [Fact]
    public void IdenticalBoxes_ProduceSameHandle()
    {
        var box1 = new Box(new Vector3(1, 2, 3));
        var box2 = new Box(new Vector3(1, 2, 3));

        var handle1 = new MeshHandle(box1);
        var handle2 = new MeshHandle(box2);

        Assert.Equal(handle1, handle2);
    }

    [Fact]
    public void DifferentBoxes_ProduceDifferentHandles()
    {
        var box1 = new Box(new Vector3(1, 2, 3));
        var box2 = new Box(new Vector3(1, 2, 4));

        var handle1 = new MeshHandle(box1);
        var handle2 = new MeshHandle(box2);

        Assert.NotEqual(handle1, handle2);
    }

    [Fact]
    public void IdenticalCylinders_ProduceSameHandle()
    {
        var cylinder1 = new Cylinder(0.5f, 1.0f, 16);
        var cylinder2 = new Cylinder(0.5f, 1.0f, 16);

        var handle1 = new MeshHandle(cylinder1);
        var handle2 = new MeshHandle(cylinder2);

        Assert.Equal(handle1, handle2);
    }

    [Fact]
    public void DifferentCylinders_ProduceDifferentHandles()
    {
        var cylinder1 = new Cylinder(0.5f, 1.0f, 16);
        var cylinder2 = new Cylinder(0.6f, 1.0f, 16);

        var handle1 = new MeshHandle(cylinder1);
        var handle2 = new MeshHandle(cylinder2);

        Assert.NotEqual(handle1, handle2);
    }

    [Fact]
    public void IdenticalSpheres_ProduceSameHandle()
    {
        var sphere1 = new Sphere(0.5f, 16, 16);
        var sphere2 = new Sphere(0.5f, 16, 16);

        var handle1 = new MeshHandle(sphere1);
        var handle2 = new MeshHandle(sphere2);

        Assert.Equal(handle1, handle2);
    }

    [Fact]
    public void IdenticalHemispheres_ProduceSameHandle()
    {
        var hemisphere_1 = new Hemisphere(0.5f, 16, 16);
        var hemisphere_2 = new Hemisphere(0.5f, 16, 16);

        var handle1 = new MeshHandle(hemisphere_1);
        var handle2 = new MeshHandle(hemisphere_2);

        Assert.Equal(handle1, handle2);
    }

    [Fact]
    public void DifferentPrimitiveTypes_ProduceDifferentHandles()
    {
        var box = new Box(new Vector3(1, 1, 1));
        var sphere = new Sphere(0.5f);

        var handleBox = new MeshHandle(box);
        var handleSphere = new MeshHandle(sphere);

        Assert.NotEqual(handleBox, handleSphere);
    }

    [Fact]
    public void InterfaceConstructor_WorksCorrectly()
    {
        IGeometryPrimitive primitive = new Cylinder(0.5f, 1.0f, 16);
        var handleFromInterface = new MeshHandle(primitive);

        var cylinder = new Cylinder(0.5f, 1.0f, 16);
        var handleFromConcrete = new MeshHandle(cylinder);

        Assert.Equal(handleFromInterface, handleFromConcrete);
    }

    [Fact]
    public void LocalPositionAndRotation_DoNotAffectHandle()
    {
        var box1 = new Box(new Vector3(1, 2, 3), new Vector3(10, 20, 30), Quaternion.Identity);
        var box2 = new Box(
            new Vector3(1, 2, 3),
            new Vector3(999, 999, 999),
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI)
        );

        var handle1 = new MeshHandle(box1);
        var handle2 = new MeshHandle(box2);

        // Position/rotation shouldn't affect mesh identity
        Assert.Equal(handle1, handle2);
    }
}
