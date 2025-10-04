using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Primitives;
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
    public override IGeometryRig GeometryRig => GetGeometryRig(config, GetColor(Biome, ChunkType));

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

    /// <summary>
    /// All terrain chunks have the same geometry, which is determined by this static method.
    /// </summary>
    /// <returns>The geometry rig for a terrain chunk.</returns>
    public static IGeometryRig GetGeometryRig(TerrainConfig config, Color color) =>
        Box.Cube(config.ChunkSize, new Material(color)).AsStaticRig();

    /// <summary>
    /// Figures out the color of the terrain chunk based on the biome and the chunk type.
    /// </summary>
    /// <returns>The color of the terrain chunk.</returns>
    public static Color GetColor(Biome biome, TerrainChunkType chunkType)
    {
        return biome switch
        {
            Biome.Meadow
                => chunkType switch
                {
                    TerrainChunkType.Grass => Color.LightGreen_400,
                    TerrainChunkType.Sand => Color.Yellow_400,
                    TerrainChunkType.Water => Color.LightBlue_300,
                    _ => throw new NotImplementedException(),
                },
            Biome.Snow
                => chunkType switch
                {
                    TerrainChunkType.Grass => Color.BlueGrey_100,
                    TerrainChunkType.Sand => Color.BlueGrey_300,
                    TerrainChunkType.Water => Color.Grey_200,
                    _ => throw new NotImplementedException(),
                },
            _ => throw new NotImplementedException(),
        };
    }
}
