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
    /// The position of this rig in world space.
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// The rotation of this rig in world space.
    /// </summary>
    Quaternion Rotation { get; }
}
