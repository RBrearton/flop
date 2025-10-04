using System.Numerics;

namespace Flop.Core;

/// <summary>
/// A static entity is an entity that does not interact and cannot be interacted with.
/// This is used for things like rocks, the terrain, grass etc.
/// This will likely also be used in spell effects etc.
/// </summary>
public abstract class StaticEntity(Identity identity, Vector3 position, Quaternion rotation)
    : Entity(identity, position, rotation) { }
