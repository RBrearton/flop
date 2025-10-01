using System.Numerics;
using Flop.Core.Geometry.Primitives;
using Raylib_cs;

namespace Flop.Core.Geometry.Primitives;

/// <summary>
/// A geometry component representing a cylinder.
/// </summary>
public readonly record struct Cylinder : IGeometryPrimitive
{
    public float Radius { get; init; }
    public float Height { get; init; }
    public int Slices { get; init; }
    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    public float Diameter => Radius * 2;

    public Cylinder(
        float radius,
        float height,
        int slices = 16,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
    {
        Radius = radius;
        Height = height;
        Slices = slices;
        LocalPosition = localPosition == default ? Vector3.Zero : localPosition;
        LocalRotation = localRotation == default ? Quaternion.Identity : localRotation;
    }

    #region IGeometryPrimitive
    public Mesh GetMesh(IMeshGenerator generator) =>
        generator.GenMeshCylinder(Radius, Height, Slices);

    public Box BoundingBox =>
        new(new Vector3(Diameter, Height, Diameter), LocalPosition, LocalRotation);
    #endregion
}
