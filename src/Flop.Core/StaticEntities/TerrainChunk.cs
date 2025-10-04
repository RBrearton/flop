using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Noise;
using Flop.Core.StaticEntities.Terrain;

namespace Flop.Core.StaticEntities;

/// <summary>
/// Represents a single chunk of procedurally generated terrain.
/// </summary>
public class TerrainChunk(
    TerrainIndex index,
    TerrainChunkType chunkType,
    Biome biome,
    TerrainConfig config
)
    : StaticEntity(
        Identity.New("TerrainChunk", index.ToString()),
        index.ToPosition(config.ChunkSize),
        Quaternion.Identity
    )
{
    public TerrainIndex Index { get; } = index;
    public TerrainChunkType ChunkType { get; } = chunkType;
    public Biome Biome { get; } = biome;

    public override IGeometryRig GeometryRig => throw new NotImplementedException();

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

        return new TerrainChunk(index, chunkType, biome, config);
    }
}
