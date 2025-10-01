using System.Numerics;
using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// Interface specifying what we expect from an individual component of our modular geometry system.
/// </summary>
public interface IGeometryComponent
{
    /// <summary>
    /// Return the Raylib Mesh for this geometry module.
    /// </summary>
    Mesh GetMesh();

    /// <summary>
    /// Return the bounding box for this geometry module.
    /// </summary>
    Box GetBoundingBox();

    /// <summary>
    /// Return the center point for this geometry module.
    /// </summary>
    Vector3 GetCenter() => GetBoundingBox().Size / 2;
}
