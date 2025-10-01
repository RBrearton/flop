using System.Numerics;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Geometry;

/// <summary>
/// A geometry component representing a capsule (cylinder with hemispherical ends).
/// Composed of a cylinder body with two hemisphere caps.
/// </summary>
public readonly record struct Capsule : IGeometryComponent
{
    public float Radius { get; init; }
    public float Height { get; init; }
    public int Slices { get; init; }
    public int Rings { get; init; }
    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    public float Diameter => Radius * 2;
    public float TotalHeight => Height + Diameter;

    public Capsule(
        float radius,
        float height,
        int slices = 16,
        int rings = 8,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
    {
        Radius = radius;
        Height = height;
        Slices = slices;
        Rings = rings;
        LocalPosition = localPosition == default ? Vector3.Zero : localPosition;
        LocalRotation = localRotation == default ? Quaternion.Identity : localRotation;
    }

    #region IGeometryComponent
    public IReadOnlyList<IGeometryPrimitive> Primitives
    {
        get
        {
            var halfHeight = Height / 2;
            return
            [
                // Cylinder body at center
                new Cylinder(Radius, Height, Slices, LocalPosition, LocalRotation),
                // Top hemisphere
                new Hemisphere(
                    Radius,
                    Rings,
                    Slices,
                    LocalPosition + new Vector3(0, halfHeight, 0),
                    LocalRotation
                ),
                // Bottom hemisphere (rotated 180 degrees)
                new Hemisphere(
                    Radius,
                    Rings,
                    Slices,
                    LocalPosition + new Vector3(0, -halfHeight, 0),
                    LocalRotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI)
                ),
            ];
        }
    }

    public Box BoundingBox =>
        new(new Vector3(Diameter, TotalHeight, Diameter), LocalPosition, LocalRotation);
    #endregion
}
