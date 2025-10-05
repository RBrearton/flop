using System.Collections.Concurrent;
using System.Numerics;

namespace Flop.Core.Networking;

/// <summary>
/// Production implementation that communicates with the game server.
/// Uses HTTP for requests and WebSocket for server pushes.
/// </summary>
public class NetworkGameStateStream : IGameStateStream
{
    private readonly ConcurrentQueue<Message> _messages = new();
    private readonly HttpClient _http;
    private readonly CancellationTokenSource _cts = new();

    public NetworkGameStateStream(string serverUrl)
    {
        _http = new HttpClient { BaseAddress = new Uri(serverUrl) };

        // Start WebSocket listener for server pushes
        _ = ListenForServerPushesAsync(_cts.Token);
    }

    public async Task RequestInitialStateAsync()
    {
        // TODO: Implement HTTP request to /api/state
        // var response = await _http.GetFromJsonAsync<Message>("/api/state");
        // _messages.Enqueue(response);
        await Task.CompletedTask;
    }

    public async Task RequestNearbyActorsAsync(Vector3 position, float radius)
    {
        // TODO: Implement HTTP request to /api/actors/nearby
        await Task.CompletedTask;
    }

    public bool TryDequeue(out Message message)
    {
        return _messages.TryDequeue(out message!);
    }

    private async Task ListenForServerPushesAsync(CancellationToken ct)
    {
        // TODO: Implement WebSocket connection
        // while (!ct.IsCancellationRequested)
        // {
        //     var message = await webSocket.ReceiveAsync<Message>(ct);
        //     _messages.Enqueue(message);
        // }
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _http.Dispose();
    }
}
