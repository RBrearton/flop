using System.Numerics;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Geometry;

/// <summary>
/// Interface for a geometry rig - a collection of components forming an actor.
/// Examples: animated character with separate body parts, vehicle with moving parts.
/// </summary>
public interface IGeometryRig
{
    /// <summary>
    /// The geometry components that make up this rig.
    /// </summary>
    IReadOnlyList<IGeometryComponent> Components { get; }

    /// <summary>
    /// Return the bounding box for this entire rig.
    /// </summary>
    Box BoundingBox { get; }

    /// <summary>
    /// Recursively flatten this rig to all constituent primitives.
    /// </summary>
    /// <returns>All primitives in all components of this rig.</returns>
    IEnumerable<IGeometryPrimitive> AllPrimitives()
    {
        foreach (var component in Components)
        {
            foreach (var primitive in component.Primitives)
            {
                yield return primitive;
            }
        }
    }
}
