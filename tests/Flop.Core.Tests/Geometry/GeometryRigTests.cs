using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Components;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Tests.Geometry;

public class GeometryRigTests
{
    private static readonly Material TestMaterial = Material.Default;

    [Fact]
    public void AllPrimitives_SingleComponent_YieldsAllPrimitives()
    {
        var capsule = new Capsule(0.5f, 2.0f, TestMaterial);
        IGeometryRig rig = new TestRig([capsule]);

        var primitives = rig.AllPrimitives().ToList();

        Assert.Equal(3, primitives.Count); // Capsule has 3 primitives
    }

    [Fact]
    public void AllPrimitives_MultipleComponents_YieldsAllPrimitives()
    {
        var capsule = new Capsule(0.5f, 2.0f, TestMaterial); // 3 primitives
        var sphere = new Sphere(1.0f, TestMaterial); // 1 primitive (also a component via wrapper)
        IGeometryRig rig = new TestRig([capsule, new SinglePrimitiveComponent(sphere)]);

        var primitives = rig.AllPrimitives().ToList();

        Assert.Equal(4, primitives.Count);
    }

    [Fact]
    public void AllPrimitives_EmptyRig_YieldsNothing()
    {
        IGeometryRig rig = new TestRig([]);

        var primitives = rig.AllPrimitives().ToList();

        Assert.Empty(primitives);
    }

    [Fact]
    public void AllPrimitives_PreservesOrder()
    {
        var cylinder = new Cylinder(0.5f, 2.0f, TestMaterial);
        var sphere = new Sphere(1.0f, TestMaterial);
        var box = new Box(1, 2, 3, TestMaterial);

        IGeometryRig rig = new TestRig(
            [
                new SinglePrimitiveComponent(cylinder),
                new SinglePrimitiveComponent(sphere),
                new SinglePrimitiveComponent(box),
            ]
        );

        var primitives = rig.AllPrimitives().ToList();

        Assert.Equal(3, primitives.Count);
        Assert.IsType<Cylinder>(primitives[0]);
        Assert.IsType<Sphere>(primitives[1]);
        Assert.IsType<Box>(primitives[2]);
    }

    // Simple test rig implementation
    private class TestRig(IReadOnlyList<IGeometryComponent> components) : IGeometryRig
    {
        public IReadOnlyList<IGeometryComponent> Components => components;
        public AxisAlignedBoundingBox BoundingBox => Box.Cube(1, TestMaterial).BoundingBox;
        public static Vector3 Position => Vector3.Zero;
        public static Quaternion Rotation => Quaternion.Identity;
    }

    // Wrapper to make a primitive act as a component
    private class SinglePrimitiveComponent(IGeometryPrimitive primitive) : IGeometryComponent
    {
        public IReadOnlyList<IGeometryPrimitive> Primitives => [primitive];
        public AxisAlignedBoundingBox BoundingBox => primitive.BoundingBox;
        public Vector3 LocalPosition => primitive.LocalPosition;
        public Quaternion LocalRotation => primitive.LocalRotation;
    }
}
