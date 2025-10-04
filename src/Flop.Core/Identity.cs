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

    /// <summary>
    /// Create a new identity with a new UniqueId given the UniqueId's prefix and a display name.
    /// </summary>
    /// <param name="uniqueIdPrefix">The prefix for the unique ID.</param>
    /// <param name="displayName">The display name for the identity.</param>
    /// <returns>The new identity.</returns>
    public static Identity New(string uniqueIdPrefix, string displayName) =>
        new(displayName, UniqueId.New(uniqueIdPrefix));
}
