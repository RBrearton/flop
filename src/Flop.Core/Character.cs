using System.Numerics;

namespace Flop.Core;

/// <summary>
/// The base class for all players and NPCs in the game.
/// </summary>
public abstract class Character(
    Identity identity,
    Vector3 position,
    Quaternion rotation,
    HealthFactor hitPoints,
    HealthFactor bloodLoss,
    HealthFactor poison
) : Actor(identity, position, rotation)
{
    public HealthFactor HitPoints { get; } = hitPoints;
    public HealthFactor BloodLoss { get; } = bloodLoss;
    public HealthFactor Poison { get; } = poison;
}
