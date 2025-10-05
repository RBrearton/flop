namespace Flop.Core.Networking.Payloads;

/// <summary>
/// Payload for InitialState message containing all nearby actors.
/// </summary>
public record InitialStatePayload(IReadOnlyList<Actor> Actors);
