using Raylib_cs;

namespace Flop.Client.Rendering;

/// <summary>
/// Interface for loading and unloading materials to/from GPU.
/// Abstracts raylib calls for testability.
/// </summary>
public interface IMaterialLoader
{
    /// <summary>
    /// Load a material with the given color.
    /// </summary>
    Material LoadMaterial(Flop.Core.Color color);

    /// <summary>
    /// Unload a material from GPU.
    /// </summary>
    void UnloadMaterial(Material material);
}
