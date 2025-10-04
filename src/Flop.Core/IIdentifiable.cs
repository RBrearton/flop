namespace Flop.Core;

/// <summary>
/// An interface for all things that need to be identified in the game.
/// </summary>
public interface IIdentifiable
{
    /// <summary>
    /// This object's identity.
    /// </summary>
    Identity Identity { get; }

    /// <summary>
    /// The unique identifier associated with this object.
    /// </summary>
    UniqueId UniqueId => Identity.UniqueId;

    /// <summary>
    /// The unique ID string associated with this object.
    /// </summary>
    string UniqueIdString => Identity.UniqueId.ToString();

    /// <summary>
    /// This object's display name.
    /// </summary>
    string DisplayName => Identity.DisplayName;
}
