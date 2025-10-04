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
}
