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
    public Material Material { get; init; }

    /// <summary>
    /// The diameter of the cylinder (2 * Radius).
    /// </summary>
    public float Diameter => Radius * 2;

    /// <summary>
    /// Create a cylinder with the specified dimensions.
    /// </summary>
    /// <param name="radius">The radius of the cylinder.</param>
    /// <param name="height">The height of the cylinder.</param>
    /// <param name="material">The material for this cylinder.</param>
    /// <param name="slices">The number of circumferential subdivisions. Defaults to 16.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
    public Cylinder(
        float radius,
        float height,
        Material material,
        int slices = 16,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
    {
        Radius = radius;
        Height = height;
        Material = material;
        Slices = slices;
        LocalPosition = localPosition == default ? Vector3.Zero : localPosition;
        LocalRotation = localRotation == default ? Quaternion.Identity : localRotation;
    }

    #region IGeometryPrimitive
    public Mesh GetMesh(IMeshGenerator generator) =>
        generator.GenMeshCylinder(Radius, Height, Slices);

    /// <summary>
    /// The axis-aligned bounding box for this cylinder.
    /// Creates a rotated box that snugly fits the cylinder, then returns its AABB.
    /// </summary>
    public AxisAlignedBoundingBox BoundingBox
    {
        get
        {
            // Create an oriented bounding box (OBB) that exactly encompasses the cylinder.
            var orientedBoundingBox = new Box(
                Diameter,
                Height,
                Diameter,
                Material,
                LocalPosition,
                LocalRotation
            );

            // Get the AABB of the OBB.
            return orientedBoundingBox.BoundingBox;
        }
    }
    #endregion
}
