namespace Flop.Core;

/// <summary>
/// An identity for a uniquely identifiable thing in the game.
/// This is a combination of a display name and a unique identifier.
/// </summary>
public readonly record struct Identity(string DisplayName, UniqueId UniqueId)
{
    /// <summary>
    /// The display name of this identity.
    /// </summary>
    public string DisplayName { get; init; } = DisplayName;

    /// <summary>
    /// The unique identifier of this identity.
    /// </summary>
    public UniqueId UniqueId { get; init; } = UniqueId;
}
