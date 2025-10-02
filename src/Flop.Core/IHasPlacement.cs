using System.Numerics;

namespace Flop.Core;

/// <summary>
/// An interface for objects that have a placement in the game world.
/// </summary>
public interface IHasPlacement
{
    /// <summary>
    /// The position of the object in the game world.
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// The rotation of the object in the game world.
    /// </summary>
    Quaternion Rotation { get; }
}
