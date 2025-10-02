namespace Flop.Core;

/// <summary>
/// Represents a material for rendering.
/// Currently supports only solid colors, but can be extended with textures and shader properties.
/// </summary>
public readonly record struct Material(Color Color)
{
    /// <summary>
    /// Default white material.
    /// </summary>
    public static readonly Material Default = new(Color.White);
}
