namespace Flop.Core;

/// <summary>
/// The interface that must be implemented by all things in the game that need to be associated with
/// a description.
/// This is super useful for building up the game's UI.
/// </summary>
public interface IDescribable
{
    /// <summary>
    /// The description of the thing.
    /// </summary>
    string Description { get; }
}
