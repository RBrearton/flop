using Flop.Core.Geometry;

namespace Flop.Core;

/// <summary>
/// An interface for objects that can be rendered.
/// </summary>
public interface IRenderable : IHasPlacement
{
    /// <summary>
    /// The geometry rig that we'll render.
    /// </summary>
    IGeometryRig GeometryRig { get; }
}
