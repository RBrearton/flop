using System.Numerics;
using Flop.Core.Geometry;

namespace Flop.Core.Tests.Geometry;

public class AxisAlignedBoundingBoxTests
{
    [Fact]
    public void Constructor_SetsMinMax()
    {
        var min = new Vector3(1, 2, 3);
        var max = new Vector3(4, 5, 6);
        var aabb = new AxisAlignedBoundingBox(min, max);

        Assert.Equal(min, aabb.Min);
        Assert.Equal(max, aabb.Max);
    }

    [Fact]
    public void Center_IsAverageBetweenMinAndMax()
    {
        var aabb = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(10, 20, 30));

        Assert.Equal(new Vector3(5, 10, 15), aabb.Center);
    }

    [Fact]
    public void Size_IsDifferenceBetweenMaxAndMin()
    {
        var aabb = new AxisAlignedBoundingBox(new Vector3(1, 2, 3), new Vector3(4, 7, 10));

        Assert.Equal(new Vector3(3, 5, 7), aabb.Size);
    }

    [Fact]
    public void Extents_IsHalfSize()
    {
        var aabb = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(10, 20, 30));

        Assert.Equal(new Vector3(5, 10, 15), aabb.Extents);
    }

    [Fact]
    public void FromCenterAndSize_CreatesCorrectAABB()
    {
        var center = new Vector3(5, 5, 5);
        var size = new Vector3(4, 6, 8);
        var aabb = AxisAlignedBoundingBox.FromCenterAndSize(center, size);

        Assert.Equal(new Vector3(3, 2, 1), aabb.Min);
        Assert.Equal(new Vector3(7, 8, 9), aabb.Max);
        Assert.Equal(center, aabb.Center);
        Assert.Equal(size, aabb.Size);
    }

    [Fact]
    public void FromCenterAndExtents_CreatesCorrectAABB()
    {
        var center = new Vector3(5, 5, 5);
        var extents = new Vector3(2, 3, 4);
        var aabb = AxisAlignedBoundingBox.FromCenterAndExtents(center, extents);

        Assert.Equal(new Vector3(3, 2, 1), aabb.Min);
        Assert.Equal(new Vector3(7, 8, 9), aabb.Max);
        Assert.Equal(center, aabb.Center);
        Assert.Equal(extents, aabb.Extents);
    }

    [Fact]
    public void FromPoints_SinglePoint_CreatesZeroSizeAABB()
    {
        var point = new Vector3(1, 2, 3);
        var aabb = AxisAlignedBoundingBox.FromPoints([point]);

        Assert.Equal(point, aabb.Min);
        Assert.Equal(point, aabb.Max);
        Assert.Equal(Vector3.Zero, aabb.Size);
    }

    [Fact]
    public void FromPoints_MultiplePoints_CreatesEnclosingAABB()
    {
        var points = new[]
        {
            new Vector3(1, 2, 3),
            new Vector3(4, 1, 6),
            new Vector3(2, 5, 2),
            new Vector3(3, 3, 4),
        };
        var aabb = AxisAlignedBoundingBox.FromPoints(points);

        Assert.Equal(new Vector3(1, 1, 2), aabb.Min);
        Assert.Equal(new Vector3(4, 5, 6), aabb.Max);
    }

    [Fact]
    public void FromPoints_EmptyList_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => AxisAlignedBoundingBox.FromPoints([]));
    }

    [Fact]
    public void Union_CreatesEnclosingAABB()
    {
        var a = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(2, 2, 2));
        var b = new AxisAlignedBoundingBox(new Vector3(1, 1, 1), new Vector3(3, 3, 3));
        var union = AxisAlignedBoundingBox.Union(a, b);

        Assert.Equal(new Vector3(0, 0, 0), union.Min);
        Assert.Equal(new Vector3(3, 3, 3), union.Max);
    }

    [Fact]
    public void Union_NonOverlapping_CreatesEnclosingAABB()
    {
        var a = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        var b = new AxisAlignedBoundingBox(new Vector3(5, 5, 5), new Vector3(6, 6, 6));
        var union = AxisAlignedBoundingBox.Union(a, b);

        Assert.Equal(new Vector3(0, 0, 0), union.Min);
        Assert.Equal(new Vector3(6, 6, 6), union.Max);
    }

    [Fact]
    public void Contains_PointInside_ReturnsTrue()
    {
        var aabb = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(10, 10, 10));

        Assert.True(aabb.Contains(new Vector3(5, 5, 5)));
    }

    [Fact]
    public void Contains_PointOnBoundary_ReturnsTrue()
    {
        var aabb = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(10, 10, 10));

        Assert.True(aabb.Contains(new Vector3(0, 5, 5)));
        Assert.True(aabb.Contains(new Vector3(10, 5, 5)));
    }

    [Fact]
    public void Contains_PointOutside_ReturnsFalse()
    {
        var aabb = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(10, 10, 10));

        Assert.False(aabb.Contains(new Vector3(-1, 5, 5)));
        Assert.False(aabb.Contains(new Vector3(11, 5, 5)));
        Assert.False(aabb.Contains(new Vector3(5, -1, 5)));
    }

    [Fact]
    public void Intersects_Overlapping_ReturnsTrue()
    {
        var a = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(5, 5, 5));
        var b = new AxisAlignedBoundingBox(new Vector3(3, 3, 3), new Vector3(8, 8, 8));

        Assert.True(a.Intersects(b));
        Assert.True(b.Intersects(a));
    }

    [Fact]
    public void Intersects_Touching_ReturnsTrue()
    {
        var a = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(5, 5, 5));
        var b = new AxisAlignedBoundingBox(new Vector3(5, 0, 0), new Vector3(10, 5, 5));

        Assert.True(a.Intersects(b));
    }

    [Fact]
    public void Intersects_Separated_ReturnsFalse()
    {
        var a = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(5, 5, 5));
        var b = new AxisAlignedBoundingBox(new Vector3(10, 10, 10), new Vector3(15, 15, 15));

        Assert.False(a.Intersects(b));
        Assert.False(b.Intersects(a));
    }
}
