using System.Numerics;
using Flop.Core.Geometry;

namespace Flop.Core;

/// <summary>
/// The base class for all renderable things in the game.
/// </summary>
public abstract class Entity(Identity identity, Vector3 position, Quaternion rotation)
    : IRenderable,
        IIdentifiable
{
    /// <summary>
    /// The entity's geometry rig.
    /// This contains all the rigid bodies (geometry components) that make up the entity.
    /// </summary>
    public abstract IGeometryRig GeometryRig { get; }

    /// <summary>
    /// The entity's position in the game world.
    /// </summary>
    public Vector3 Position { get; } = position;

    /// <summary>
    /// The entity's rotation in the game world.
    /// </summary>
    public Quaternion Rotation { get; } = rotation;

    /// <summary>
    /// The entity's identity.
    /// </summary>
    public Identity Identity { get; } = identity;
}
