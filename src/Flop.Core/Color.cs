namespace Flop.Core;

/// <summary>
/// Represents an RGBA color with 8-bit channels.
/// </summary>
public readonly record struct Color(byte R, byte G, byte B, byte A = 255)
{
    /// <summary>
    /// White color (255, 255, 255, 255).
    /// </summary>
    public static readonly Color White = new(255, 255, 255);

    /// <summary>
    /// Black color (0, 0, 0, 255).
    /// </summary>
    public static readonly Color Black = new(0, 0, 0);

    /// <summary>
    /// Red color (255, 0, 0, 255).
    /// </summary>
    public static readonly Color Red = new(255, 0, 0);

    /// <summary>
    /// Green color (0, 255, 0, 255).
    /// </summary>
    public static readonly Color Green = new(0, 255, 0);

    /// <summary>
    /// Blue color (0, 0, 255, 255).
    /// </summary>
    public static readonly Color Blue = new(0, 0, 255);

    /// <summary>
    /// Yellow color (255, 255, 0, 255).
    /// </summary>
    public static readonly Color Yellow = new(255, 255, 0);

    /// <summary>
    /// Cyan color (0, 255, 255, 255).
    /// </summary>
    public static readonly Color Cyan = new(0, 255, 255);

    /// <summary>
    /// Magenta color (255, 0, 255, 255).
    /// </summary>
    public static readonly Color Magenta = new(255, 0, 255);

    /// <summary>
    /// Gray color (128, 128, 128, 255).
    /// </summary>
    public static readonly Color Gray = new(128, 128, 128);
}
