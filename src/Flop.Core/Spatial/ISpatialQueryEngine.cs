using System.Numerics;
using Flop.Core.Geometry;

namespace Flop.Core.Spatial;

/// <summary>
/// The interface that must be satisfied by any spatial query engine.
/// There are many spatial query algorithms, each with different trade-offs.
/// This interface provides a common way to interact with different spatial query algorithms.
/// </summary>
public interface ISpatialQueryEngine<T>
    where T : Actor
{
    /// <summary>
    /// Get all actors that are inside the given bounding box.
    /// </summary>
    /// <param name="boundingBox">The bounding box to query.</param>
    /// <returns>The actors that are inside the bounding box.</returns>
    IEnumerable<T> GetInside(AxisAlignedBoundingBox boundingBox);

    /// <summary>
    /// Get all actors that are within the given range from the given position.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="range">The range to query.</param>
    /// <returns>The actors that are within the range from the position.</returns>
    IEnumerable<T> GetInRange(Vector3 position, float range);

    /// <summary>
    /// Gets the N actors nearest to the given position.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="maxRange">The maximum range to query.
    /// If the distance is greater than this, the actor will not be returned.</param>
    /// <param name="count">The number of actors to return.</param>
    /// <returns>The actors nearest to the position.</returns>
    IEnumerable<T> GetNearest(Vector3 position, float maxRange, int count);

    /// <summary>
    /// Get the single nearest actor to the given position.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="maxRange">The maximum range to query.</param>
    /// <returns>The nearest actor, or null if none found within range.</returns>
    T? GetNearest(Vector3 position, float maxRange);

    /// <summary>
    /// Get all actors along a ray path.
    /// </summary>
    /// <param name="origin">The origin of the ray.</param>
    /// <param name="direction">The direction of the ray (should be normalized).</param>
    /// <param name="maxDistance">The maximum distance to check.</param>
    /// <returns>All actors intersecting the ray, sorted by distance.</returns>
    IEnumerable<T> RaycastAll(Vector3 origin, Vector3 direction, float maxDistance);

    /// <summary>
    /// Raycast to find the first actor intersecting a ray.
    /// </summary>
    /// <param name="origin">The origin of the ray.</param>
    /// <param name="direction">The direction of the ray (should be normalized).</param>
    /// <param name="maxDistance">The maximum distance to check.</param>
    /// <returns>The first actor hit by the ray, or null if none.</returns>
    T? Raycast(Vector3 origin, Vector3 direction, float maxDistance)
    {
        return RaycastAll(origin, direction, maxDistance).FirstOrDefault();
    }

    /// <summary>
    /// Count the number of actors within range without allocating a collection.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="range">The range to query.</param>
    /// <returns>The count of actors within range.</returns>
    int CountInRange(Vector3 position, float range)
    {
        return GetInRange(position, range).Count();
    }

    /// <summary>
    /// Check if any actors exist within the given range.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="range">The range to query.</param>
    /// <returns>True if any actors are within range.</returns>
    bool AnyInRange(Vector3 position, float range)
    {
        return GetInRange(position, range).Any();
    }
}
