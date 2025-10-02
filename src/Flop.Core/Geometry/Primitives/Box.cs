using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry.Primitives;

/// <summary>
/// A geometry primitive representing a box/cuboid.
/// </summary>
public readonly record struct Box : IGeometryPrimitive
{
    /// <summary>
    /// The dimensions of the box along each axis.
    /// </summary>
    public Vector3 Size { get; init; }

    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }
    public Material Material { get; init; }

    /// <summary>
    /// The width of the box (X dimension).
    /// </summary>
    public float SizeX => Size.X;

    /// <summary>
    /// The height of the box (Y dimension).
    /// </summary>
    public float SizeY => Size.Y;

    /// <summary>
    /// The depth of the box (Z dimension).
    /// </summary>
    public float SizeZ => Size.Z;

    /// <summary>
    /// Create a box with the specified dimensions.
    /// </summary>
    /// <param name="size">The size of the box along each axis.</param>
    /// <param name="material">The material handle for this box.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
    public Box(
        Vector3 size,
        Material material,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
    {
        Size = size;
        Material = material;
        LocalPosition = localPosition == default ? Vector3.Zero : localPosition;
        LocalRotation = localRotation == default ? Quaternion.Identity : localRotation;
    }

    /// <summary>
    /// Create a box with individual dimension parameters.
    /// </summary>
    /// <param name="size_x">The width of the box.</param>
    /// <param name="size_y">The height of the box.</param>
    /// <param name="size_z">The depth of the box.</param>
    /// <param name="material">The material handle for this box.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
    public Box(
        float size_x,
        float size_y,
        float size_z,
        Material material,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
        : this(new Vector3(size_x, size_y, size_z), material, localPosition, localRotation) { }

    /// <summary>
    /// Create a cube (uniform box) with the given size on all axes.
    /// </summary>
    /// <param name="size">The size of the cube on all axes.</param>
    /// <param name="material">The material handle for this cube.</param>
    /// <param name="localPosition">The local position offset. Defaults to origin.</param>
    /// <param name="localRotation">The local rotation. Defaults to identity.</param>
    /// <returns>A box with equal dimensions on all axes.</returns>
    public static Box Cube(
        float size,
        Material material,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    ) => new(size, size, size, material, localPosition, localRotation);

    #region IGeometryPrimitive
    public Mesh GetMesh(IMeshGenerator generator) => generator.GenMeshCube(SizeX, SizeY, SizeZ);

    public Box BoundingBox => this;
    #endregion
}
