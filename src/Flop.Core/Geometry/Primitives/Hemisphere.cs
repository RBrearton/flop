using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry.Primitives;

/// <summary>
/// A geometry primitive representing a hemisphere (half sphere).
/// The flat face lies on the XZ plane, with the dome extending in the positive Y direction.
/// </summary>
public readonly record struct Hemisphere : IGeometryPrimitive
{
    /// <summary>
    /// The radius of the hemisphere.
    /// </summary>
    public float Radius { get; init; }

    /// <summary>
    /// The number of horizontal subdivisions (latitude lines).
    /// Higher values create smoother hemispheres but increase vertex count.
    /// </summary>
    public int Rings { get; init; }

    /// <summary>
    /// The number of vertical subdivisions (longitude lines).
    /// Higher values create smoother hemispheres but increase vertex count.
    /// </summary>
    public int Slices { get; init; }

    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    /// <summary>
    /// The diameter of the hemisphere's base (2 * Radius).
    /// </summary>
    public float Diameter => Radius * 2;

    /// <summary>
    /// Create a hemisphere with the specified dimensions.
    /// </summary>
    /// <param name="radius">The radius of the hemisphere.</param>
    /// <param name="rings">The number of horizontal subdivisions.</param>
    /// <param name="slices">The number of vertical subdivisions.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
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

    #region IGeometryPrimitive
    public Mesh GetMesh(IMeshGenerator generator) =>
        generator.GenMeshHemiSphere(Radius, Rings, Slices);

    public Box BoundingBox =>
        new(new Vector3(Diameter, Radius, Diameter), LocalPosition, LocalRotation);
    #endregion
}
