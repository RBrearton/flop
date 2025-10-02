using Flop.Client.Rendering;
using Raylib_cs;

namespace Flop.Client.Tests.Mocks;

public class MockMaterialLoader : IMaterialLoader
{
    public int LoadCount { get; private set; }
    public int UnloadCount { get; private set; }

    public unsafe Material LoadMaterial(Flop.Core.Color color)
    {
        LoadCount++;
        // Return a mock material with default values
        // Note: Material.Maps is a pointer in raylib-cs 7.0, so we just return default
        return default;
    }

    public void UnloadMaterial(Material material)
    {
        UnloadCount++;
    }
}
