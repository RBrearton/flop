using System.Numerics;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Geometry.Components;

/// <summary>
/// A geometry component representing a capsule (cylinder with hemispherical ends).
/// Composed of a central cylinder body with hemisphere caps on both ends.
/// Commonly used for character collision shapes in games.
/// </summary>
public readonly record struct Capsule : IGeometryComponent
{
    /// <summary>
    /// The radius of the capsule's cylindrical body and hemispherical caps.
    /// </summary>
    public float Radius { get; init; }

    /// <summary>
    /// The height of the cylindrical body (excluding the hemisphere caps).
    /// </summary>
    public float Height { get; init; }

    /// <summary>
    /// The number of vertical subdivisions for the cylinder and hemispheres.
    /// Higher values create smoother shapes but increase vertex count.
    /// </summary>
    public int Slices { get; init; }

    /// <summary>
    /// The number of horizontal subdivisions for the hemispheres.
    /// Higher values create smoother caps but increase vertex count.
    /// </summary>
    public int Rings { get; init; }

    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }
    public Material Material { get; init; }

    /// <summary>
    /// The diameter of the capsule (2 * Radius).
    /// </summary>
    public float Diameter => Radius * 2;

    /// <summary>
    /// The total height of the capsule including both hemisphere caps.
    /// Equal to Height + Diameter (cylinder height + 2 * radius).
    /// </summary>
    public float TotalHeight => Height + Diameter;

    /// <summary>
    /// Create a capsule with the specified dimensions.
    /// </summary>
    /// <param name="radius">The radius of the capsule.</param>
    /// <param name="height">The height of the cylindrical body (excluding caps).</param>
    /// <param name="material">The material for all parts of this capsule.</param>
    /// <param name="slices">The number of vertical subdivisions. Defaults to 16.</param>
    /// <param name="rings">The number of horizontal subdivisions for caps. Defaults to 8.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
    public Capsule(
        float radius,
        float height,
        Material material,
        int slices = 16,
        int rings = 8,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
    {
        Radius = radius;
        Height = height;
        Material = material;
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
                new Cylinder(Radius, Height, Material, Slices, LocalPosition, LocalRotation),
                // Top hemisphere
                new Hemisphere(
                    Radius,
                    Material,
                    Rings,
                    Slices,
                    LocalPosition + new Vector3(0, halfHeight, 0),
                    LocalRotation
                ),
                // Bottom hemisphere (rotated 180 degrees)
                new Hemisphere(
                    Radius,
                    Material,
                    Rings,
                    Slices,
                    LocalPosition + new Vector3(0, -halfHeight, 0),
                    LocalRotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI)
                ),
            ];
        }
    }

    public Box BoundingBox =>
        new(new Vector3(Diameter, TotalHeight, Diameter), Material, LocalPosition, LocalRotation);
    #endregion
}
