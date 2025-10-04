using System.Numerics;
using Flop.Core.Geometry.Components;
using Flop.Core.Geometry.Rigs;
using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// Interface specifying what we expect from an individual component of our modular geometry system.
/// These are the fundamental building blocks of our geometry system.
/// Examples are things like boxes, spheres, cylinders etc.
/// </summary>
public interface IGeometryPrimitive
{
    /// <summary>
    /// Return the Raylib Mesh for this geometry module.
    /// This method requires an IMeshGenerator implementation to help with the low-level mesh
    /// generation.
    /// </summary>
    Mesh GetMesh(IMeshGenerator generator);

    /// <summary>
    /// Return the axis-aligned bounding box for this geometry primitive.
    /// </summary>
    AxisAlignedBoundingBox BoundingBox { get; }

    /// <summary>
    /// Return the center point for this geometry primitive.
    /// </summary>
    Vector3 Center => BoundingBox.Center;

    /// <summary>
    /// The local position of this geometry primitive.
    /// </summary>
    Vector3 LocalPosition { get; }

    /// <summary>
    /// The local rotation of this geometry primitive.
    /// </summary>
    Quaternion LocalRotation { get; }

    /// <summary>
    /// The material for this primitive.
    /// Contains rendering properties like color (and eventually textures, shaders, etc.).
    /// </summary>
    Material Material { get; }

    /// <summary>
    /// Creates a simple component consisting of only this primitive.
    /// </summary>
    /// <returns>A simple component consisting of only this primitive.</returns>
    SimpleComponent AsSimpleComponent() => new(this);

    /// <summary>
    /// Creates a simple static rig consisting of only a simple component, which in turn
    /// consists of only this primitive.
    /// </summary>
    /// <returns>A simple static rig consisting of only a simple component, which in turn
    /// consists of only this primitive.</returns>
    StaticRig AsStaticRig() => new(AsSimpleComponent());
}
