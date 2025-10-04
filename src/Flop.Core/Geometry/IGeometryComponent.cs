using System.Numerics;
using Flop.Core.Geometry.Primitives;
using Flop.Core.Geometry.Rigs;

namespace Flop.Core.Geometry;

/// <summary>
/// Interface for a geometry component - a collection of primitives forming a rigid body.
/// Examples: sword, shield, character torso.
/// </summary>
public interface IGeometryComponent
{
    /// <summary>
    /// The geometry primitives that make up this component.
    /// </summary>
    IReadOnlyList<IGeometryPrimitive> Primitives { get; }

    /// <summary>
    /// Return the axis-aligned bounding box for this geometry component.
    /// </summary>
    AxisAlignedBoundingBox BoundingBox { get; }

    /// <summary>
    /// The local position of this geometry component.
    /// </summary>
    Vector3 LocalPosition { get; }

    /// <summary>
    /// The local rotation of this geometry component.
    /// </summary>
    Quaternion LocalRotation { get; }

    /// <summary>
    /// Creates a static rig consisting only of this component.
    /// </summary>
    /// <returns>A static rig consisting only of this component.</returns>
    StaticRig AsStaticRig() => new(this);
}
