namespace Flop.Core.Actors;

/// <summary>
/// The base class for all environment actors.
/// Actors come in two flavours: environment actors and characters.
/// Characters can be NPCs or player characters, and do all the usual things like move, interact
/// etc.
///
/// Environment actors are things like trees, mining nodes, herbs, etc.
/// They're things in the world that the player can interact with, but they're unable to interact
/// with anything themselves.
/// </summary>
public abstract class EnvironmentActorBase : Actor { }
