namespace Flop.Core.Terrain;

/// <summary>
/// Configuration for procedural terrain generation.
/// </summary>
public readonly record struct TerrainConfig
{
    /// <summary>
    /// Seed for deterministic terrain generation.
    /// </summary>
    public uint Seed { get; init; }

    /// <summary>
    /// The size of each terrain chunk in world units.
    /// </summary>
    public float ChunkSize { get; init; }

    /// <summary>
    /// Noise values below this threshold generate water chunks.
    /// </summary>
    public float WaterCutoff { get; init; }

    /// <summary>
    /// Noise values below this threshold (but above water) generate sand chunks.
    /// </summary>
    public float SandCutoff { get; init; }

    /// <summary>
    /// Scale of noise features for chunk type variation.
    /// Larger values = larger regions of the same chunk type.
    /// </summary>
    public float ChunkVariationScale { get; init; }

    /// <summary>
    /// Noise values below this threshold generate meadow biomes.
    /// </summary>
    public float MeadowCutoff { get; init; }

    /// <summary>
    /// Scale of noise features for biome variation.
    /// Larger values = larger biome regions.
    /// </summary>
    public float BiomeVariationScale { get; init; }

    /// <summary>
    /// Create a terrain configuration with default values.
    /// </summary>
    public static TerrainConfig Default =>
        new()
        {
            Seed = 42,
            ChunkSize = 32.0f,
            WaterCutoff = 0.3f,
            SandCutoff = 0.4f,
            ChunkVariationScale = 100.0f,
            MeadowCutoff = 0.6f,
            BiomeVariationScale = 200.0f,
        };

    /// <summary>
    /// Get the chunk type associated with the given Perlin noise value.
    /// </summary>
    /// <param name="noiseValue">The noise value in range [0, 1].</param>
    /// <returns>The appropriate chunk type for the given noise value.</returns>
    public readonly TerrainChunkType GetChunkType(float noiseValue)
    {
        if (noiseValue < WaterCutoff)
        {
            return TerrainChunkType.Water;
        }
        else if (noiseValue < SandCutoff)
        {
            return TerrainChunkType.Sand;
        }
        else
        {
            return TerrainChunkType.Grass;
        }
    }

    /// <summary>
    /// Get the biome associated with the given Perlin noise value.
    /// </summary>
    /// <param name="noiseValue">The noise value in range [0, 1].</param>
    /// <returns>The appropriate biome for the given noise value.</returns>
    public readonly Biome GetBiome(float noiseValue)
    {
        if (noiseValue < MeadowCutoff)
        {
            return Biome.Meadow;
        }
        else
        {
            return Biome.Snow;
        }
    }
}
