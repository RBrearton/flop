using System.Numerics;

namespace Flop.Core.Geometry.Components;

/// <summary>
/// A SimpleComponent is a geometry component wrapping a single primitive.
/// </summary>
public class SimpleComponent(IGeometryPrimitive primitive) : CompoundComponent([primitive]) { }
