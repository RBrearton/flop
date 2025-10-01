using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry.Primitives;

/// <summary>
/// A geometry primitive representing a cylinder.
/// </summary>
public readonly record struct Cylinder : IGeometryPrimitive
{
    /// <summary>
    /// The radius of the cylinder's circular cross-section.
    /// </summary>
    public float Radius { get; init; }

    /// <summary>
    /// The height of the cylinder along its central axis.
    /// </summary>
    public float Height { get; init; }

    /// <summary>
    /// The number of subdivisions around the cylinder's circumference.
    /// Higher values create smoother cylinders but increase vertex count.
    /// </summary>
    public int Slices { get; init; }

    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    /// <summary>
    /// The diameter of the cylinder (2 * Radius).
    /// </summary>
    public float Diameter => Radius * 2;

    /// <summary>
    /// Create a cylinder with the specified dimensions.
    /// </summary>
    /// <param name="radius">The radius of the cylinder.</param>
    /// <param name="height">The height of the cylinder.</param>
    /// <param name="slices">The number of circumferential subdivisions. Defaults to 16.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
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
