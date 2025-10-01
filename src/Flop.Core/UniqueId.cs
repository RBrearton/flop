namespace Flop.Core;

/// <summary>
/// The unique identifier that we use in the Flop universe.
/// This is a combination of a prefix and a GUID.
/// </summary>
public readonly record struct UniqueId(string prefix, Guid guid)
{
    /// <summary>
    /// The prefix for the unique ID.
    /// </summary>
    public string Prefix { get; init; } = prefix;

    /// <summary>
    /// The GUID for the unique ID.
    /// </summary>
    public Guid Guid { get; init; } = guid;

    public override string ToString() => $"{Prefix}-{Guid}";

    /// <summary>
    /// Parse a unique ID from a string.
    /// </summary>
    /// <param name="id">The string representation of the unique ID.</param>
    public static UniqueId FromString(string id)
    {
        var parts = id.Split('-');
        return new(parts[0], Guid.Parse(parts[1]));
    }

    /// <summary>
    /// Create a new unique ID with the given prefix.
    /// </summary>
    /// <param name="prefix">The prefix for the unique ID.</param>
    /// <returns>The new unique ID.</returns>
    public static UniqueId New(string prefix) => new(prefix, Guid.NewGuid());
}
