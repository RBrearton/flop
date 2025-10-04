using Flop.Core.Noise;

namespace Flop.Core.Terrain;

/// <summary>
/// Represents a single chunk of procedurally generated terrain.
/// </summary>
public readonly record struct TerrainChunk
{
    /// <summary>
    /// The 2D index of this chunk in the terrain grid.
    /// </summary>
    public TerrainIndex Index { get; init; }

    /// <summary>
    /// The type of terrain in this chunk e.g. (grass, sand, water).
    /// </summary>
    public TerrainChunkType ChunkType { get; init; }

    /// <summary>
    /// The biome this chunk belongs to e.g. (meadow, snow, etc).
    /// </summary>
    public Biome Biome { get; init; }

    /// <summary>
    /// Create a new terrain chunk with the given properties.
    /// </summary>
    public TerrainChunk(TerrainIndex index, TerrainChunkType chunkType, Biome biome)
    {
        Index = index;
        ChunkType = chunkType;
        Biome = biome;
    }

    /// <summary>
    /// Procedurally generate a terrain chunk at the given index.
    /// </summary>
    /// <param name="index">The 2D index of the chunk to generate.</param>
    /// <param name="config">The terrain generation configuration.</param>
    /// <returns>A procedurally generated terrain chunk.</returns>
    public static TerrainChunk Generate(TerrainIndex index, TerrainConfig config)
    {
        float x = index.X;
        float z = index.Z;

        // Generate noise for chunk type and biome.
        float chunkNoise = PerlinNoise.Generate(x, z, config.ChunkVariationScale, config.Seed);
        float biomeNoise = PerlinNoise.Generate(x, z, config.BiomeVariationScale, config.Seed);

        // Determine chunk type and biome from noise values.
        TerrainChunkType chunkType = config.GetChunkType(chunkNoise);
        Biome biome = config.GetBiome(biomeNoise);

        return new TerrainChunk(index, chunkType, biome);
    }
}
