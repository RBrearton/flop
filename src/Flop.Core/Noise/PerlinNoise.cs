using System.Runtime.CompilerServices;

namespace Flop.Core.Noise;

/// <summary>
/// Fast Perlin noise implementation for 2D terrain generation.
/// </summary>
public static class PerlinNoise
{
    /// <summary>
    /// Generate 2D Perlin noise at the given coordinates.
    /// </summary>
    /// <param name="x">X coordinate in world space.</param>
    /// <param name="y">Y coordinate in world space.</param>
    /// <param name="lengthscale">Scale of noise features. Larger values = larger features.</param>
    /// <param name="seed">Seed for deterministic noise generation.</param>
    /// <returns>Noise value in range [0, 1].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Generate(float x, float y, float lengthscale, uint seed)
    {
        // Scale coordinates by lengthscale.
        float scaledX = x / lengthscale;
        float scaledY = y / lengthscale;

        // Find grid cell.
        int x0 = (int)MathF.Floor(scaledX);
        int y0 = (int)MathF.Floor(scaledY);
        int x1 = x0 + 1;
        int y1 = y0 + 1;

        // Get fractional part within cell.
        float fractionalX = scaledX - x0;
        float fractionalY = scaledY - y0;

        // Get gradients at four corners.
        (float gx00, float gy00) = Gradient2D(x0, y0, seed);
        (float gx10, float gy10) = Gradient2D(x1, y0, seed);
        (float gx01, float gy01) = Gradient2D(x0, y1, seed);
        (float gx11, float gy11) = Gradient2D(x1, y1, seed);

        // Calculate dot products with distance vectors.
        float d00 = Dot2D(gx00, gy00, fractionalX, fractionalY);
        float d10 = Dot2D(gx10, gy10, fractionalX - 1.0f, fractionalY);
        float d01 = Dot2D(gx01, gy01, fractionalX, fractionalY - 1.0f);
        float d11 = Dot2D(gx11, gy11, fractionalX - 1.0f, fractionalY - 1.0f);

        // Cubic interpolation.
        float u = SmoothStep(fractionalX);
        float v = SmoothStep(fractionalY);

        // Bilinear interpolation.
        float nx0 = Lerp(d00, d10, u);
        float nx1 = Lerp(d01, d11, u);
        float nxy = Lerp(nx0, nx1, v);

        // Map from [-1, 1] to [0, 1].
        return (nxy + 1.0f) * 0.5f;
    }

    /// <summary>
    /// Fast hash function for generating gradients.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Hash2D(int x, int y, uint seed)
    {
        uint hash = unchecked((uint)x * 374761393u);
        hash = unchecked((hash + (uint)y) * 668265263u);
        hash = unchecked((hash + seed) * 1274126177u);
        hash ^= hash >> 16;
        return unchecked(hash * 2246822519u);
    }

    /// <summary>
    /// Generate unit gradient vector from grid coordinates.
    /// Uses Ken Perlin's gradient selection approach for 8 evenly distributed gradients.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (float, float) Gradient2D(int x, int y, uint seed)
    {
        uint h = Hash2D(x, y, seed);

        return (h & 7) switch
        {
            0 => (1.0f, 1.0f),
            1 => (-1.0f, 1.0f),
            2 => (1.0f, -1.0f),
            3 => (-1.0f, -1.0f),
            4 => (1.0f, 0.0f),
            5 => (-1.0f, 0.0f),
            6 => (0.0f, 1.0f),
            _ => (0.0f, -1.0f),
        };
    }

    /// <summary>
    /// 2D dot product.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Dot2D(float ax, float ay, float bx, float by)
    {
        return ax * bx + ay * by;
    }

    /// <summary>
    /// Cubic smoothstep interpolation (3t² - 2t³).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float SmoothStep(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }

    /// <summary>
    /// Linear interpolation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }
}
