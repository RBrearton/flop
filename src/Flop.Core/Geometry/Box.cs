using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// A very simple geometry component representing a box/cuboid.
/// </summary>
public readonly record struct Box : IGeometryComponent
{
    /// <summary>
    /// The size of the box.
    /// </summary>
    public Vector3 Size { get; }
    public float SizeX => Size.X;
    public float SizeY => Size.Y;
    public float SizeZ => Size.Z;

    public Box(Vector3 size)
    {
        Size = size;
    }

    public Box(float size_x, float size_y, float size_z)
    {
        Size = new Vector3(size_x, size_y, size_z);
    }

    /// <summary>
    /// Create a cube with the given size.
    /// </summary>
    public static Box Cube(float size) => new(size, size, size);

    #region IGeometryComponent
    public Mesh GetMesh()
    {
        return new Mesh();
    }

    public Box BoundingBox => this;
    #endregion
}
