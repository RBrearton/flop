namespace Flop.Core.Geometry.Rigs;

/// <summary>
/// A static rig is the simplest kind of geometry rig - it just contains a single geometry
/// component.
/// </summary>
public class StaticRig(IGeometryComponent component) : IGeometryRig
{
    public IReadOnlyList<IGeometryComponent> Components { get; } = [component];
    public AxisAlignedBoundingBox BoundingBox { get; } = component.BoundingBox;
}
