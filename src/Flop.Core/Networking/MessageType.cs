namespace Flop.Core.Networking;

/// <summary>
/// Types of messages that can be sent from server to client.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Initial state with all nearby actors.
    /// </summary>
    InitialState,

    /// <summary>
    /// A new actor was added to the world.
    /// </summary>
    ActorAdded,

    /// <summary>
    /// An actor was removed from the world.
    /// </summary>
    ActorRemoved,

    /// <summary>
    /// An existing actor was updated.
    /// </summary>
    ActorUpdated,
}
