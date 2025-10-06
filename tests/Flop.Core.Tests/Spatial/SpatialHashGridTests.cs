using System.Numerics;
using Flop.Core.Actors.Trees;
using Flop.Core.Geometry;
using Flop.Core.Spatial;

namespace Flop.Core.Tests.Spatial;

public class SpatialHashGridTests
{
    private static WillowTree CreateTree(string name, float x, float z)
    {
        return new WillowTree(
            Identity.New("tree", name),
            new Vector3(x, 0, z),
            Quaternion.Identity
        );
    }

    #region Constructor and Basic Operations

    [Fact]
    public void Constructor_WithEmptyList_CreatesEmptyGrid()
    {
        var grid = new SpatialHashGrid<WillowTree>([], 10f);

        Assert.Equal(10f, grid.CellLength);
        Assert.Empty(grid.GetAll());
    }

    [Fact]
    public void Constructor_WithActors_StoresAllActors()
    {
        var actors = new List<WillowTree>
        {
            CreateTree("Tree1", 0, 0),
            CreateTree("Tree2", 5, 5),
            CreateTree("Tree3", 10, 10),
        };

        var grid = new SpatialHashGrid<WillowTree>(actors, 10f);

        Assert.Equal(3, grid.GetAll().Count);
    }

    [Fact]
    public void AddActor_AddsActorToGrid()
    {
        var grid = new SpatialHashGrid<WillowTree>([], 10f);
        var tree = CreateTree("Tree1", 5, 5);

        grid.AddActor(tree);

        Assert.Single(grid.GetAll());
        Assert.Contains(tree, grid.GetAll());
    }

    [Fact]
    public void RemoveActor_ByReference_RemovesActor()
    {
        var tree = CreateTree("Tree1", 5, 5);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        grid.RemoveActor(tree);

        Assert.Empty(grid.GetAll());
    }

    [Fact]
    public void RemoveActor_ByIdentity_RemovesActor()
    {
        var tree = CreateTree("Tree1", 5, 5);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        grid.RemoveActor(tree.Identity);

        Assert.Empty(grid.GetAll());
    }

    [Fact]
    public void RemoveActor_OnlyRemovesSpecifiedActor_LeavesOthersInSameCell()
    {
        // CRITICAL: Multiple actors in the same cell
        var tree1 = CreateTree("Tree1", 1, 1);
        var tree2 = CreateTree("Tree2", 2, 2);
        var tree3 = CreateTree("Tree3", 3, 3);

        var grid = new SpatialHashGrid<WillowTree>([tree1, tree2, tree3], 10f);

        // All three should be in the same cell (cell size is 10)
        grid.RemoveActor(tree2);

        var remaining = grid.GetAll();
        Assert.Equal(2, remaining.Count);
        Assert.Contains(tree1, remaining);
        Assert.DoesNotContain(tree2, remaining);
        Assert.Contains(tree3, remaining);
    }

    [Fact]
    public void RemoveActor_NonExistentActor_DoesNotThrow()
    {
        var grid = new SpatialHashGrid<WillowTree>([], 10f);
        var tree = CreateTree("Tree1", 5, 5);

        // Should not throw when removing actor that's not in grid
        grid.RemoveActor(tree);
        grid.RemoveActor(tree.Identity);

        Assert.Empty(grid.GetAll());
    }

    [Fact]
    public void RemoveActor_RemovingLastActorInCell_CleansUpCell()
    {
        var tree = CreateTree("Tree1", 5, 5);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        grid.RemoveActor(tree);

        // Should be able to query without issues
        Assert.Empty(grid.GetInRange(new Vector3(5, 0, 5), 10f));
    }

    [Fact]
    public void GetActorFromIdentity_ExistingActor_ReturnsActor()
    {
        var tree = CreateTree("Tree1", 5, 5);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        var result = grid.GetActorFromIdentity(tree.Identity);

        Assert.NotNull(result);
        Assert.Equal(tree, result);
    }

    [Fact]
    public void GetActorFromIdentity_NonExistentActor_ReturnsNull()
    {
        var grid = new SpatialHashGrid<WillowTree>([], 10f);

        var result = grid.GetActorFromIdentity(Identity.New("tree", "Nonexistent"));

        Assert.Null(result);
    }

    [Fact]
    public void TryUpdateActorPosition_SameCell_ReturnsFalse()
    {
        var tree = CreateTree("Tree1", 5, 5);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        // Move within same cell (cell size is 10, so 5->7 is same cell)
        var movedTree = new WillowTree(
            tree.Identity,
            new Vector3(7, 0, 7),
            Quaternion.Identity
        );

        var result = grid.TryUpdateActorPosition(movedTree);

        Assert.False(result);
    }

    [Fact]
    public void TryUpdateActorPosition_DifferentCell_ReturnsTrue()
    {
        var tree = CreateTree("Tree1", 5, 5);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        // Move to different cell (5->15 crosses cell boundary)
        var movedTree = new WillowTree(
            tree.Identity,
            new Vector3(15, 0, 15),
            Quaternion.Identity
        );

        var result = grid.TryUpdateActorPosition(movedTree);

        Assert.True(result);
    }

    [Fact]
    public void GetActorsNear_ReturnsAdjacentActors()
    {
        var center = CreateTree("Center", 5, 5);
        var nearby = CreateTree("Nearby", 7, 7);
        var far = CreateTree("Far", 100, 100);

        var grid = new SpatialHashGrid<WillowTree>([center, nearby, far], 10f);

        var result = grid.GetActorsNear(center);

        Assert.Contains(nearby, result);
        Assert.DoesNotContain(far, result);
        Assert.DoesNotContain(center, result); // Should not include itself
    }

    #endregion

    #region GetInside

    [Fact]
    public void GetInside_EmptyGrid_ReturnsEmpty()
    {
        var grid = new SpatialHashGrid<WillowTree>([], 10f);
        var bbox = new AxisAlignedBoundingBox(Vector3.Zero, new Vector3(10, 10, 10));

        var result = grid.GetInside(bbox);

        Assert.Empty(result);
    }

    [Fact]
    public void GetInside_ActorsInside_ReturnsThoseActors()
    {
        var inside1 = CreateTree("Inside1", 5, 5);
        var inside2 = CreateTree("Inside2", 7, 7);
        var outside = CreateTree("Outside", 15, 15);

        var grid = new SpatialHashGrid<WillowTree>([inside1, inside2, outside], 10f);
        var bbox = new AxisAlignedBoundingBox(Vector3.Zero, new Vector3(10, 10, 10));

        var result = grid.GetInside(bbox).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(inside1, result);
        Assert.Contains(inside2, result);
        Assert.DoesNotContain(outside, result);
    }

    [Fact]
    public void GetInside_ActorOnBoundary_ReturnsActor()
    {
        var onBoundary = CreateTree("OnBoundary", 10, 10);
        var grid = new SpatialHashGrid<WillowTree>([onBoundary], 10f);
        var bbox = new AxisAlignedBoundingBox(Vector3.Zero, new Vector3(10, 10, 10));

        var result = grid.GetInside(bbox);

        Assert.Contains(onBoundary, result);
    }

    #endregion

    #region GetInRange

    [Fact]
    public void GetInRange_EmptyGrid_ReturnsEmpty()
    {
        var grid = new SpatialHashGrid<WillowTree>([], 10f);

        var result = grid.GetInRange(Vector3.Zero, 10f);

        Assert.Empty(result);
    }

    [Fact]
    public void GetInRange_ActorsWithinRange_ReturnsThoseActors()
    {
        var near = CreateTree("Near", 3, 0);
        var far = CreateTree("Far", 50, 0);

        var grid = new SpatialHashGrid<WillowTree>([near, far], 10f);

        var result = grid.GetInRange(Vector3.Zero, 10f).ToList();

        Assert.Single(result);
        Assert.Contains(near, result);
        Assert.DoesNotContain(far, result);
    }

    [Fact]
    public void GetInRange_ExactlyAtRange_IncludesActor()
    {
        var atRange = CreateTree("AtRange", 10, 0);
        var grid = new SpatialHashGrid<WillowTree>([atRange], 10f);

        var result = grid.GetInRange(Vector3.Zero, 10f);

        Assert.Contains(atRange, result);
    }

    [Fact]
    public void GetInRange_MultipleActorsSameCell_ReturnsAll()
    {
        var tree1 = CreateTree("Tree1", 1, 1);
        var tree2 = CreateTree("Tree2", 2, 2);
        var tree3 = CreateTree("Tree3", 3, 3);

        var grid = new SpatialHashGrid<WillowTree>([tree1, tree2, tree3], 10f);

        var result = grid.GetInRange(Vector3.Zero, 5f).ToList();

        Assert.Equal(3, result.Count);
    }

    #endregion

    #region GetNearest

    [Fact]
    public void GetNearest_Single_EmptyGrid_ReturnsNull()
    {
        var grid = new SpatialHashGrid<WillowTree>([], 10f);

        var result = grid.GetNearest(Vector3.Zero, 10f);

        Assert.Null(result);
    }

    [Fact]
    public void GetNearest_Single_ReturnsClosestActor()
    {
        var close = CreateTree("Close", 2, 0);
        var far = CreateTree("Far", 5, 0);

        var grid = new SpatialHashGrid<WillowTree>([close, far], 10f);

        var result = grid.GetNearest(Vector3.Zero, 10f);

        Assert.Equal(close, result);
    }

    [Fact]
    public void GetNearest_Multiple_ReturnsRequestedCount()
    {
        var actors = new List<WillowTree>
        {
            CreateTree("Tree1", 1, 0),
            CreateTree("Tree2", 2, 0),
            CreateTree("Tree3", 3, 0),
            CreateTree("Tree4", 4, 0),
            CreateTree("Tree5", 5, 0),
        };

        var grid = new SpatialHashGrid<WillowTree>(actors, 10f);

        var result = grid.GetNearest(Vector3.Zero, 10f, 3).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetNearest_Multiple_ReturnsSortedByDistance()
    {
        var tree1 = CreateTree("Tree1", 3, 0);
        var tree2 = CreateTree("Tree2", 1, 0);
        var tree3 = CreateTree("Tree3", 2, 0);

        var grid = new SpatialHashGrid<WillowTree>([tree1, tree2, tree3], 10f);

        var result = grid.GetNearest(Vector3.Zero, 10f, 3).ToList();

        Assert.Equal(tree2, result[0]); // Distance 1
        Assert.Equal(tree3, result[1]); // Distance 2
        Assert.Equal(tree1, result[2]); // Distance 3
    }

    [Fact]
    public void GetNearest_Multiple_BeyondMaxRange_ReturnsEmpty()
    {
        var far = CreateTree("Far", 100, 0);
        var grid = new SpatialHashGrid<WillowTree>([far], 10f);

        var result = grid.GetNearest(Vector3.Zero, 10f, 5);

        Assert.Empty(result);
    }

    #endregion

    #region Raycast

    [Fact]
    public void Raycast_EmptyGrid_ReturnsNull()
    {
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([], 10f);

        var result = grid.Raycast(Vector3.Zero, Vector3.UnitX, 10f);

        Assert.Null(result);
    }

    [Fact]
    public void Raycast_ActorOnRay_ReturnsActor()
    {
        var onRay = CreateTree("OnRay", 5, 0);
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([onRay], 10f);

        var result = grid.Raycast(Vector3.Zero, Vector3.UnitX, 10f);

        Assert.Equal(onRay, result);
    }

    [Fact]
    public void Raycast_ActorBeyondMaxDistance_ReturnsNull()
    {
        var far = CreateTree("Far", 50, 0);
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([far], 10f);

        var result = grid.Raycast(Vector3.Zero, Vector3.UnitX, 10f);

        Assert.Null(result);
    }

    [Fact]
    public void RaycastAll_MultipleActorsOnRay_ReturnsSortedByDistance()
    {
        var near = CreateTree("Near", 2, 0);
        var middle = CreateTree("Middle", 5, 0);
        var far = CreateTree("Far", 8, 0);

        var grid = new SpatialHashGrid<WillowTree>([middle, far, near], 10f);

        var result = grid.RaycastAll(Vector3.Zero, Vector3.UnitX, 10f).ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal(near, result[0]);
        Assert.Equal(middle, result[1]);
        Assert.Equal(far, result[2]);
    }

    #endregion

    #region CountInRange and AnyInRange

    [Fact]
    public void CountInRange_EmptyGrid_ReturnsZero()
    {
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([], 10f);

        var count = grid.CountInRange(Vector3.Zero, 10f);

        Assert.Equal(0, count);
    }

    [Fact]
    public void CountInRange_MultipleActors_ReturnsCorrectCount()
    {
        var actors = new List<WillowTree>
        {
            CreateTree("Tree1", 1, 0),
            CreateTree("Tree2", 2, 0),
            CreateTree("Tree3", 3, 0),
            CreateTree("Tree4", 50, 0), // Far away
        };

        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>(actors, 10f);

        var count = grid.CountInRange(Vector3.Zero, 10f);

        Assert.Equal(3, count);
    }

    [Fact]
    public void AnyInRange_EmptyGrid_ReturnsFalse()
    {
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([], 10f);

        var any = grid.AnyInRange(Vector3.Zero, 10f);

        Assert.False(any);
    }

    [Fact]
    public void AnyInRange_ActorsInRange_ReturnsTrue()
    {
        var tree = CreateTree("Tree", 5, 0);
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        var any = grid.AnyInRange(Vector3.Zero, 10f);

        Assert.True(any);
    }

    [Fact]
    public void AnyInRange_NoActorsInRange_ReturnsFalse()
    {
        var tree = CreateTree("Tree", 50, 0);
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        var any = grid.AnyInRange(Vector3.Zero, 10f);

        Assert.False(any);
    }

    #endregion

    #region Raycast Edge Cases

    [Fact]
    public void Raycast_RayParallelToAxis_WorksCorrectly()
    {
        var tree = CreateTree("Tree", 5, 0);
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        // Ray parallel to X axis
        var result = grid.Raycast(Vector3.Zero, Vector3.UnitX, 10f);

        Assert.NotNull(result);
        Assert.Equal(tree, result);
    }

    [Fact]
    public void Raycast_RayStartingInsideActor_ReturnsActor()
    {
        var tree = CreateTree("Tree", 0, 0);
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        // Start ray from inside the actor's bounding box
        var result = grid.Raycast(Vector3.Zero, Vector3.UnitX, 10f);

        Assert.NotNull(result);
        Assert.Equal(tree, result);
    }

    [Fact]
    public void Raycast_RayInOppositeDirection_DoesNotReturnActor()
    {
        var tree = CreateTree("Tree", 5, 0);
        ISpatialQueryEngine<WillowTree> grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        // Ray pointing away from actor
        var result = grid.Raycast(Vector3.Zero, -Vector3.UnitX, 10f);

        Assert.Null(result);
    }

    [Fact]
    public void RaycastAll_PreservesOrderByDistance()
    {
        // Ensure actors are returned in correct distance order
        var near = CreateTree("Near", 1, 0);
        var far = CreateTree("Far", 9, 0);
        var middle = CreateTree("Middle", 5, 0);

        var grid = new SpatialHashGrid<WillowTree>([far, near, middle], 10f);

        var result = grid.RaycastAll(Vector3.Zero, Vector3.UnitX, 20f).ToList();

        // Should be ordered by distance
        Assert.True(result.Count >= 2);
        var nearIndex = result.IndexOf(near);
        var middleIndex = result.IndexOf(middle);
        var farIndex = result.IndexOf(far);

        Assert.True(nearIndex < middleIndex);
        Assert.True(middleIndex < farIndex);
    }

    #endregion

    #region GetNearest Edge Cases

    [Fact]
    public void GetNearest_TieBreaking_ReturnsConsistently()
    {
        // Two actors equidistant - should return consistently
        var tree1 = CreateTree("Tree1", 5, 0);
        var tree2 = CreateTree("Tree2", 0, 5);

        var grid = new SpatialHashGrid<WillowTree>([tree1, tree2], 10f);

        var result1 = grid.GetNearest(Vector3.Zero, 10f);
        var result2 = grid.GetNearest(Vector3.Zero, 10f);

        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal(result1, result2); // Should be consistent
    }

    [Fact]
    public void GetNearest_WithCount_NeverReturnsMoreThanRequested()
    {
        var actors = new List<WillowTree>();
        for (int i = 0; i < 20; i++)
        {
            actors.Add(CreateTree($"Tree{i}", i, 0));
        }

        var grid = new SpatialHashGrid<WillowTree>(actors, 10f);

        var result = grid.GetNearest(Vector3.Zero, 100f, 5).ToList();

        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void GetNearest_WithCountLargerThanAvailable_ReturnsAllAvailable()
    {
        var tree1 = CreateTree("Tree1", 1, 0);
        var tree2 = CreateTree("Tree2", 2, 0);

        var grid = new SpatialHashGrid<WillowTree>([tree1, tree2], 10f);

        var result = grid.GetNearest(Vector3.Zero, 10f, 10).ToList();

        Assert.Equal(2, result.Count);
    }

    #endregion

    #region Position Update Edge Cases

    [Fact]
    public void TryUpdateActorPosition_MovingAcrossMultipleCells_Works()
    {
        var tree = CreateTree("Tree", 5, 5);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        // Move far away, crossing many cells
        var movedTree = new WillowTree(
            tree.Identity,
            new Vector3(105, 0, 105),
            Quaternion.Identity
        );

        grid.TryUpdateActorPosition(movedTree);

        // Should be findable at new location
        var found = grid.GetActorFromIdentity(tree.Identity);
        Assert.NotNull(found);

        // Should be in range at new location
        var nearby = grid.GetInRange(new Vector3(105, 0, 105), 5f);
        Assert.Contains(found, nearby);

        // Should NOT be in range at old location
        var oldLocation = grid.GetInRange(new Vector3(5, 0, 5), 5f);
        Assert.DoesNotContain(found, oldLocation);
    }

    #endregion

    #region Zero and Negative Coordinate Edge Cases

    [Fact]
    public void GetInRange_AtOrigin_WorksCorrectly()
    {
        var tree = CreateTree("Tree", 0, 0);
        var grid = new SpatialHashGrid<WillowTree>([tree], 10f);

        var result = grid.GetInRange(Vector3.Zero, 5f);

        Assert.Contains(tree, result);
    }

    [Fact]
    public void GetInRange_NegativeCoordinates_WorksCorrectly()
    {
        var tree1 = CreateTree("Tree1", -5, -5);
        var tree2 = CreateTree("Tree2", -3, -3);

        var grid = new SpatialHashGrid<WillowTree>([tree1, tree2], 10f);

        var result = grid.GetInRange(new Vector3(-5, 0, -5), 5f).ToList();

        Assert.Contains(tree1, result);
        Assert.Contains(tree2, result);
    }

    [Fact]
    public void GetInRange_CrossingZero_WorksCorrectly()
    {
        var neg = CreateTree("Neg", -2, -2);
        var pos = CreateTree("Pos", 2, 2);

        var grid = new SpatialHashGrid<WillowTree>([neg, pos], 10f);

        // Query centered at origin should find both
        var result = grid.GetInRange(Vector3.Zero, 5f).ToList();

        Assert.Contains(neg, result);
        Assert.Contains(pos, result);
    }

    [Fact]
    public void ActorsAtCellBoundaries_AreHandledCorrectly()
    {
        // Test actors exactly on cell boundaries
        var onBoundary1 = CreateTree("Boundary1", 10, 0);
        var onBoundary2 = CreateTree("Boundary2", 0, 10);
        var onBoundary3 = CreateTree("Boundary3", -10, 0);

        var grid = new SpatialHashGrid<WillowTree>(
            [onBoundary1, onBoundary2, onBoundary3],
            10f
        );

        // All should be findable
        Assert.NotNull(grid.GetActorFromIdentity(onBoundary1.Identity));
        Assert.NotNull(grid.GetActorFromIdentity(onBoundary2.Identity));
        Assert.NotNull(grid.GetActorFromIdentity(onBoundary3.Identity));

        // Should be in correct cells
        var near1 = grid.GetInRange(new Vector3(10, 0, 0), 2f);
        Assert.Contains(onBoundary1, near1);
    }

    #endregion

    #region Edge Cases and Performance

    [Fact]
    public void LargeCellSize_ActorsFarApart_StillWorks()
    {
        var actors = new List<WillowTree>
        {
            CreateTree("Tree1", 0, 0),
            CreateTree("Tree2", 100, 100),
            CreateTree("Tree3", 200, 200),
        };

        var grid = new SpatialHashGrid<WillowTree>(actors, 50f);

        var result = grid.GetInRange(Vector3.Zero, 150f).ToList();

        Assert.Contains(actors[0], result);
        Assert.Contains(actors[1], result);
        Assert.DoesNotContain(actors[2], result);
    }

    [Fact]
    public void SmallCellSize_ManyActors_StillWorks()
    {
        var actors = new List<WillowTree>();
        for (int i = 0; i < 100; i++)
        {
            actors.Add(CreateTree($"Tree{i}", i, i));
        }

        var grid = new SpatialHashGrid<WillowTree>(actors, 1f);

        Assert.Equal(100, grid.GetAll().Count);
        var nearby = grid.GetInRange(new Vector3(50, 0, 50), 5f).ToList();
        Assert.NotEmpty(nearby);
    }

    [Fact]
    public void NegativeCoordinates_WorksCorrectly()
    {
        var actors = new List<WillowTree>
        {
            CreateTree("NegX", -5, 0),
            CreateTree("NegZ", 0, -5),
            CreateTree("NegBoth", -5, -5),
            CreateTree("Pos", 5, 5),
        };

        var grid = new SpatialHashGrid<WillowTree>(actors, 10f);

        var result = grid.GetInRange(new Vector3(-5, 0, -5), 8f).ToList();

        Assert.Contains(actors[0], result);
        Assert.Contains(actors[1], result);
        Assert.Contains(actors[2], result);
        Assert.DoesNotContain(actors[3], result);
    }

    [Fact]
    public void ActorsAtCellBoundaries_HandledCorrectly()
    {
        var onBoundary = CreateTree("OnBoundary", 10, 10);
        var grid = new SpatialHashGrid<WillowTree>([onBoundary], 10f);

        var result = grid.GetInRange(new Vector3(10, 0, 10), 1f);

        Assert.Contains(onBoundary, result);
    }

    [Fact]
    public void AddMultipleActors_ToSameCell_AllRetrievable()
    {
        var tree1 = CreateTree("Tree1", 1, 1);
        var tree2 = CreateTree("Tree2", 2, 2);
        var tree3 = CreateTree("Tree3", 3, 3);

        var grid = new SpatialHashGrid<WillowTree>([], 10f);
        grid.AddActor(tree1);
        grid.AddActor(tree2);
        grid.AddActor(tree3);

        var found1 = grid.GetActorFromIdentity(tree1.Identity);
        var found2 = grid.GetActorFromIdentity(tree2.Identity);
        var found3 = grid.GetActorFromIdentity(tree3.Identity);

        Assert.NotNull(found1);
        Assert.NotNull(found2);
        Assert.NotNull(found3);
        Assert.Equal(tree1, found1);
        Assert.Equal(tree2, found2);
        Assert.Equal(tree3, found3);
    }

    #endregion
}
