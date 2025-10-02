namespace Flop.Core.Terrain;

/// <summary>
/// The type of terrain in a chunk.
/// </summary>
public enum TerrainChunkType
{
    /// <summary>
    /// The most common thing we see in the terrain.
    /// </summary>
    Grass,

    /// <summary>
    /// Sand is the chunk type that tends to surround water.
    /// </summary>
    Sand,

    /// <summary>
    /// There are localized bodies of water; you can't path through water.
    /// In a volcano biome this would be lava, in snowy biomes this would be ice etc.
    /// </summary>
    Water,
}
