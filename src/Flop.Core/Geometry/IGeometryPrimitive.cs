using System.Numerics;
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
    /// Return the bounding box for this geometry module.
    /// </summary>
    Box BoundingBox { get; }

    /// <summary>
    /// Return the center point for this geometry module.
    /// </summary>
    Vector3 Center => BoundingBox.Size / 2;

    /// <summary>
    /// The local position of this geometry module.
    /// </summary>
    Vector3 LocalPosition { get; }

    /// <summary>
    /// The local rotation of this geometry module.
    /// </summary>
    Quaternion LocalRotation { get; }
}
