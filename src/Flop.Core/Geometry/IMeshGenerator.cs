using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// Interface for generating geometry meshes.
/// Abstracts mesh generation for testability.
/// </summary>
public interface IMeshGenerator
{
    /// <summary>
    /// Generate a cube mesh.
    /// </summary>
    /// <param name="width">The width of the cube.</param>
    /// <param name="height">The height of the cube.</param>
    /// <param name="length">The length of the cube.</param>
    Mesh GenMeshCube(float width, float height, float length);

    /// <summary>
    /// Generate a sphere mesh.
    /// </summary>
    /// <param name="radius">The radius of the sphere.</param>
    /// <param name="rings">The number of rings.</param>
    /// <param name="slices">The number of slices.</param>
    Mesh GenMeshSphere(float radius, int rings, int slices);

    /// <summary>
    /// Generate a hemisphere mesh.
    /// </summary>
    /// <param name="radius">The radius of the hemisphere.</param>
    /// <param name="rings">The number of rings.</param>
    /// <param name="slices">The number of slices.</param>
    Mesh GenMeshHemiSphere(float radius, int rings, int slices);

    /// <summary>
    /// Generate a cylinder mesh.
    /// </summary>
    /// <param name="radius">The radius of the cylinder.</param>
    /// <param name="height">The height of the cylinder.</param>
    /// <param name="slices">The number of slices.</param>
    Mesh GenMeshCylinder(float radius, float height, int slices);
}
