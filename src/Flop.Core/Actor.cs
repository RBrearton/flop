using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;

namespace Flop.Core;

public abstract class Actor : IRenderable, IDescribable
{
    /// <summary>
    /// The actor's geometry rig.
    /// This contains all the rigid bodies (geometry components) that make up the actor.
    /// </summary>
    public abstract IGeometryRig GeometryRig { get; }

    /// <summary>
    /// The description of the actor.
    /// This will be used by the game's UI when necessary to provide things like tooltips etc.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// The actor's bounding box in the game world.
    /// </summary>
    public Box BoundingBox => GeometryRig.BoundingBox;

    /// <summary>
    /// The actor's absolute position in the game world.
    /// </summary>
    public Vector3 Position { get; }

    /// <summary>
    /// The actor's absolute rotation in the game world.
    /// </summary>
    public Quaternion Rotation { get; }
}
