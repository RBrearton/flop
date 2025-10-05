using System.Numerics;

namespace Flop.Core.Networking;

/// <summary>
/// Interface for game state streaming.
/// Handles both requesting state from server and consuming incoming messages.
/// </summary>
public interface IGameStateStream
{
    /// <summary>
    /// Request the initial game state.
    /// Response will be queued as a message.
    /// </summary>
    Task RequestInitialStateAsync();

    /// <summary>
    /// Request actors near a specific position.
    /// Response will be queued as messages.
    /// </summary>
    Task RequestNearbyActorsAsync(Vector3 position, float radius);

    /// <summary>
    /// Try to dequeue the next message (called by game loop on main thread).
    /// </summary>
    bool TryDequeue(out Message message);
}
