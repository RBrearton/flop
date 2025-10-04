using System.Numerics;
using Flop.Core.Geometry.Components;
using Flop.Core.Geometry.Rigs;
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
    public Vector3 Size { get; }

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
    /// Get the 8 corner vertices in local space (centered at origin, before rotation/translation).
    /// </summary>
    private Vector3[] GetLocalCorners()
    {
        var halfSize = Size / 2f;
        return
        [
            new(-halfSize.X, -halfSize.Y, -halfSize.Z),
            new(-halfSize.X, -halfSize.Y, halfSize.Z),
            new(-halfSize.X, halfSize.Y, -halfSize.Z),
            new(-halfSize.X, halfSize.Y, halfSize.Z),
            new(halfSize.X, -halfSize.Y, -halfSize.Z),
            new(halfSize.X, -halfSize.Y, halfSize.Z),
            new(halfSize.X, halfSize.Y, -halfSize.Z),
            new(halfSize.X, halfSize.Y, halfSize.Z),
        ];
    }

    /// <summary>
    /// Get all vertices in world space (after rotation and translation).
    /// Rotation is applied around origin, then translation is applied.
    /// </summary>
    public Vector3[] VertexCoordinates
    {
        get
        {
            var corners = GetLocalCorners();
            var rotation = LocalRotation;
            var position = LocalPosition;

            // Rotate around origin, then translate
            return [.. corners.Select(c => Vector3.Transform(c, rotation) + position)];
        }
    }

    /// <summary>
    /// The geometric center of this box.
    /// </summary>
    public Vector3 Center => LocalPosition;

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

    public SimpleComponent AsSimpleComponent() => new(this);

    public StaticRig AsStaticRig() => new(AsSimpleComponent());

    /// <summary>
    /// The axis-aligned bounding box for this box primitive.
    /// If rotated, the AABB will be larger than the box itself.
    /// </summary>
    public AxisAlignedBoundingBox BoundingBox
    {
        get
        {
            // If no rotation, AABB is exact.
            if (LocalRotation == Quaternion.Identity)
            {
                var halfSize = Size / 2f;
                return new AxisAlignedBoundingBox(
                    LocalPosition - halfSize,
                    LocalPosition + halfSize
                );
            }

            // If rotated, compute AABB from transformed corners.
            var vertices = VertexCoordinates;
            return AxisAlignedBoundingBox.FromPoints(vertices);
        }
    }
    #endregion
}
