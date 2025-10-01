using System.Numerics;
using Flop.Core.Geometry;

namespace Flop.Core;

public abstract class Actor
{
    /// <summary>
    /// The actor's geometry rig.
    /// This contains all the rigid bodies (geometry components) that make up the actor.
    /// </summary>
    public abstract IGeometryRig GeometryRig { get; }

    /// <summary>
    /// The actor's absolute position in the game world.
    /// </summary>
    public abstract Vector3 Position { get; }

    /// <summary>
    /// The actor's absolute rotation in the game world.
    /// </summary>
    public abstract Quaternion Rotation { get; }
}
