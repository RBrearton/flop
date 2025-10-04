using Flop.Core.Geometry.Primitives;

namespace Flop.Core.Geometry;

/// <summary>
/// A static rig is the simplest kind of geometry rig - it just contains a single geometry
/// component.
/// </summary>
public class StaticRig(IGeometryComponent component) : IGeometryRig
{
    public IReadOnlyList<IGeometryComponent> Components { get; } = [component];
    public Box BoundingBox { get; } = component.BoundingBox;
}
