using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// A geometry component representing a hemisphere (half sphere).
/// </summary>
public readonly record struct Hemisphere : IGeometryPrimitive
{
    public float Radius { get; init; }
    public int Rings { get; init; }
    public int Slices { get; init; }
    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    public float Diameter => Radius * 2;

    public Hemisphere(
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

    #region IGeometryComponent
    public Mesh GetMesh() => Raylib.GenMeshHemiSphere(Radius, Rings, Slices);

    public Box BoundingBox =>
        new(new Vector3(Diameter, Radius, Diameter), LocalPosition, LocalRotation);
    #endregion
}
