using System.Collections.Concurrent;
using System.Numerics;
using Flop.Core.Actors.Trees;
using Flop.Core.Networking.Payloads;

namespace Flop.Core.Networking;

/// <summary>
/// Mock implementation for offline testing.
/// Generates predetermined game state useful for testing specific scenarios.
/// </summary>
public class MockGameStateStream : IGameStateStream
{
    private readonly ConcurrentQueue<Message> _messages = new();

    public Task RequestInitialStateAsync()
    {
        // Generate a simple test scene with a few trees
        var actors = new Actor[]
        {
            new WillowTree(
                Identity.New("tree", "Willow 1"),
                new Vector3(5, 0, 5),
                Quaternion.Identity
            ),
            new OakTree(Identity.New("tree", "Oak 1"), new Vector3(-3, 0, 8), Quaternion.Identity),
            new WillowTree(
                Identity.New("tree", "Willow 2"),
                new Vector3(10, 0, -2),
                Quaternion.Identity
            ),
        };

        var message = Message.Create(MessageType.InitialState, new InitialStatePayload(actors));
        _messages.Enqueue(message);

        return Task.CompletedTask;
    }

    public Task RequestNearbyActorsAsync(Vector3 position, float radius)
    {
        // Mock: just return empty initial state for now
        var message = Message.Create(MessageType.InitialState, new InitialStatePayload([]));
        _messages.Enqueue(message);
        return Task.CompletedTask;
    }

    public bool TryDequeue(out Message message)
    {
        return _messages.TryDequeue(out message!);
    }
}
