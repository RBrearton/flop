using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry.Primitives;

/// <summary>
/// A geometry primitive representing a sphere.
/// </summary>
public readonly record struct Sphere : IGeometryPrimitive
{
    /// <summary>
    /// The radius of the sphere.
    /// </summary>
    public float Radius { get; init; }

    /// <summary>
    /// The number of horizontal subdivisions (latitude lines).
    /// Higher values create smoother spheres but increase vertex count.
    /// </summary>
    public int Rings { get; init; }

    /// <summary>
    /// The number of vertical subdivisions (longitude lines).
    /// Higher values create smoother spheres but increase vertex count.
    /// </summary>
    public int Slices { get; init; }

    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    /// <summary>
    /// The diameter of the sphere (2 * Radius).
    /// </summary>
    public float Diameter => Radius * 2;

    /// <summary>
    /// Create a sphere with the specified dimensions.
    /// </summary>
    /// <param name="radius">The radius of the sphere.</param>
    /// <param name="rings">The number of horizontal subdivisions. Defaults to 16.</param>
    /// <param name="slices">The number of vertical subdivisions. Defaults to 16.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
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
    public Mesh GetMesh(IMeshGenerator generator) => generator.GenMeshSphere(Radius, Rings, Slices);

    public Box BoundingBox =>
        new(new Vector3(Diameter, Diameter, Diameter), LocalPosition, LocalRotation);
    #endregion
}
