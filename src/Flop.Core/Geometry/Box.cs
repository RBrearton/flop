using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// A very simple geometry component representing a box/cuboid.
/// </summary>
public readonly record struct Box : IGeometryPrimitive
{
    public Vector3 Size { get; init; }
    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalRotation { get; init; }

    public float SizeX => Size.X;
    public float SizeY => Size.Y;
    public float SizeZ => Size.Z;

    public Box(Vector3 size, Vector3 localPosition = default, Quaternion localRotation = default)
    {
        Size = size;
        LocalPosition = localPosition == default ? Vector3.Zero : localPosition;
        LocalRotation = localRotation == default ? Quaternion.Identity : localRotation;
    }

    public Box(
        float size_x,
        float size_y,
        float size_z,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    )
        : this(new Vector3(size_x, size_y, size_z), localPosition, localRotation) { }

    /// <summary>
    /// Create a cube with the given size.
    /// </summary>
    public static Box Cube(
        float size,
        Vector3 localPosition = default,
        Quaternion localRotation = default
    ) => new(size, size, size, localPosition, localRotation);

    #region IGeometryComponent
    public Mesh GetMesh() => Raylib.GenMeshCube(SizeX, SizeY, SizeZ);

    public Box BoundingBox => this;
    #endregion
}
