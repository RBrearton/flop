using System.Numerics;

namespace Flop.Core.Terrain;

/// <summary>
/// A 2D index that uniquely specifies a chunk of terrain.
/// X and Z represent horizontal plane coordinates (Y is up).
/// </summary>
public readonly record struct TerrainIndex(int X, int Z)
{
    /// <summary>
    /// Get the terrain index from a 2D position.
    /// </summary>
    /// <param name="x">The X coordinate in world space.</param>
    /// <param name="z">The Z coordinate in world space.</param>
    /// <param name="chunkSize">The size of each terrain chunk.</param>
    /// <returns>The terrain index containing the given position.</returns>
    public static TerrainIndex FromPosition(float x, float z, float chunkSize)
    {
        return new TerrainIndex((int)MathF.Floor(x / chunkSize), (int)MathF.Floor(z / chunkSize));
    }

    /// <summary>
    /// Convert this terrain index to a world position.
    /// </summary>
    /// <param name="chunkSize">The size of each terrain chunk.</param>
    /// <returns>The world position at the origin of this chunk (Y=0).</returns>
    public Vector3 ToPosition(float chunkSize)
    {
        return new Vector3(X * chunkSize, 0.0f, Z * chunkSize);
    }
}
