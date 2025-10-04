using System.Numerics;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Geometry.Components;

/// <summary>
/// A SimpleComponent is a geometry component wrapping a single primitive.
/// </summary>
public class SimpleComponent(IGeometryPrimitive primitive) : IGeometryComponent
{
    public IReadOnlyList<IGeometryPrimitive> Primitives => [primitive];
    public Box BoundingBox => primitive.BoundingBox;
    public Vector3 LocalPosition => primitive.LocalPosition;
    public Quaternion LocalRotation => primitive.LocalRotation;
}
