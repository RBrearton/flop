using System.Text.Json;

namespace Flop.Core.Networking;

/// <summary>
/// A message from the server containing a type and serialized payload.
/// </summary>
public record Message(MessageType Type, string PayloadJson)
{
    /// <summary>
    /// Deserialize the payload to the specified type.
    /// </summary>
    public T DeserializePayload<T>()
    {
        return JsonSerializer.Deserialize<T>(PayloadJson)
            ?? throw new InvalidOperationException($"Failed to deserialize payload for {Type}");
    }

    /// <summary>
    /// Create a message with a serialized payload.
    /// </summary>
    public static Message Create<T>(MessageType type, T payload)
    {
        var json = JsonSerializer.Serialize(payload);
        return new Message(type, json);
    }
}
