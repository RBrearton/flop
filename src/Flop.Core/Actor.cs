using System.Numerics;

namespace Flop.Core;

public abstract class Actor(Identity identity, Vector3 position, Quaternion rotation)
    : Entity(identity, position, rotation),
        IDescribable
{
    /// <summary>
    /// The description of the actor.
    /// This will be used by the game's UI when necessary to provide things like tooltips etc.
    /// </summary>
    public abstract string Description { get; }
}
