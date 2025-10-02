using Raylib_cs;

namespace Flop.Client.Rendering;

/// <summary>
/// Raylib implementation of IMaterialLoader.
/// Creates materials using raylib's default material and sets colors.
/// </summary>
public class RaylibMaterialLoader : IMaterialLoader
{
    public unsafe Material LoadMaterial(Flop.Core.Color color)
    {
        var material = Raylib.LoadMaterialDefault();

        // Set the diffuse color in the material
        // MaterialMapIndex.Diffuse = 0, so we set the color for the diffuse map
        unsafe
        {
            material.Maps[0].Color = new Color(color.R, color.G, color.B, color.A);
        }

        return material;
    }

    public void UnloadMaterial(Material material)
    {
        Raylib.UnloadMaterial(material);
    }
}
