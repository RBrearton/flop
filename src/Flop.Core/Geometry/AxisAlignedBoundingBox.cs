using System.Numerics;

namespace Flop.Core.Geometry;

/// <summary>
/// Represents an axis-aligned bounding box (AABB) defined by minimum and maximum points.
/// AABBs are commonly used for spatial queries, collision detection, and frustum culling.
/// </summary>
public readonly record struct AxisAlignedBoundingBox
{
    /// <summary>
    /// The minimum corner of the bounding box.
    /// </summary>
    public Vector3 Min { get; init; }

    /// <summary>
    /// The maximum corner of the bounding box.
    /// </summary>
    public Vector3 Max { get; init; }

    /// <summary>
    /// The geometric center of the bounding box.
    /// </summary>
    public Vector3 Center => (Min + Max) / 2f;

    /// <summary>
    /// The size of the bounding box (Max - Min).
    /// </summary>
    public Vector3 Size => Max - Min;

    /// <summary>
    /// The half-size (extents) of the bounding box.
    /// </summary>
    public Vector3 Extents => Size / 2f;

    /// <summary>
    /// Create an AABB from minimum and maximum points.
    /// </summary>
    /// <param name="min">The minimum corner.</param>
    /// <param name="max">The maximum corner.</param>
    public AxisAlignedBoundingBox(Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
    }

    /// <summary>
    /// Create an AABB from a center point and size.
    /// </summary>
    /// <param name="center">The center of the bounding box.</param>
    /// <param name="size">The size along each axis.</param>
    /// <returns>An AABB centered at the given point with the given size.</returns>
    public static AxisAlignedBoundingBox FromCenterAndSize(Vector3 center, Vector3 size)
    {
        var halfSize = size / 2f;
        return new AxisAlignedBoundingBox(center - halfSize, center + halfSize);
    }

    /// <summary>
    /// Create an AABB from a center point and extents (half-size).
    /// </summary>
    /// <param name="center">The center of the bounding box.</param>
    /// <param name="extents">The half-size along each axis.</param>
    /// <returns>An AABB centered at the given point with the given extents.</returns>
    public static AxisAlignedBoundingBox FromCenterAndExtents(Vector3 center, Vector3 extents)
    {
        return new AxisAlignedBoundingBox(center - extents, center + extents);
    }

    /// <summary>
    /// Create an AABB that contains all the given points.
    /// </summary>
    /// <param name="points">The points to enclose.</param>
    /// <returns>The smallest AABB containing all points.</returns>
    /// <exception cref="ArgumentException">Thrown if points is empty.</exception>
    public static AxisAlignedBoundingBox FromPoints(IEnumerable<Vector3> points)
    {
        var pointList = points.ToList();
        if (pointList.Count == 0)
        {
            throw new ArgumentException("Cannot create AABB from empty point set", nameof(points));
        }

        var min = pointList[0];
        var max = pointList[0];

        foreach (var point in pointList)
        {
            min = Vector3.Min(min, point);
            max = Vector3.Max(max, point);
        }

        return new AxisAlignedBoundingBox(min, max);
    }

    /// <summary>
    /// Compute the union of two AABBs (smallest AABB containing both).
    /// </summary>
    /// <param name="a">The first AABB.</param>
    /// <param name="b">The second AABB.</param>
    /// <returns>The smallest AABB containing both input AABBs.</returns>
    public static AxisAlignedBoundingBox Union(AxisAlignedBoundingBox a, AxisAlignedBoundingBox b)
    {
        return new AxisAlignedBoundingBox(Vector3.Min(a.Min, b.Min), Vector3.Max(a.Max, b.Max));
    }

    /// <summary>
    /// Check if this AABB contains a point.
    /// </summary>
    /// <param name="point">The point to test.</param>
    /// <returns>True if the point is inside or on the boundary of the AABB.</returns>
    public bool Contains(Vector3 point)
    {
        return point.X >= Min.X
            && point.X <= Max.X
            && point.Y >= Min.Y
            && point.Y <= Max.Y
            && point.Z >= Min.Z
            && point.Z <= Max.Z;
    }

    /// <summary>
    /// Check if this AABB intersects another AABB.
    /// </summary>
    /// <param name="other">The other AABB to test.</param>
    /// <returns>True if the AABBs overlap.</returns>
    public bool Intersects(AxisAlignedBoundingBox other)
    {
        return Min.X <= other.Max.X
            && Max.X >= other.Min.X
            && Min.Y <= other.Max.Y
            && Max.Y >= other.Min.Y
            && Min.Z <= other.Max.Z
            && Max.Z >= other.Min.Z;
    }
}
