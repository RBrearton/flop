using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// A geometry component representing a sphere.
/// </summary>
public readonly record struct Sphere : IGeometryPrimitive
{
    public float Radius { get; init; }
    public int Rings { get; init; }
    public int Slices { get; init; }
    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    public float Diameter => Radius * 2;

    public Sphere(
        float radius,
        int rings = 16,
        int slices = 16,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
    {
        Radius = radius;
        Rings = rings;
        Slices = slices;
        LocalPosition = localPosition == default ? Vector3.Zero : localPosition;
        LocalRotation = localRotation == default ? Quaternion.Identity : localRotation;
    }

    #region IGeometryPrimitive
    public Mesh GetMesh(IMeshGenerator generator) =>
        generator.GenMeshSphere(Radius, Rings, Slices);

    public Box BoundingBox =>
        new(new Vector3(Diameter, Diameter, Diameter), LocalPosition, LocalRotation);
    #endregion
}
