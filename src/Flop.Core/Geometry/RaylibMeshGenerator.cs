using Raylib_cs;

namespace Flop.Core.Geometry;

/// <summary>
/// Raylib implementation of IMeshGenerator.
/// Generates meshes using raylib's built-in mesh generation functions.
/// </summary>
public class RaylibMeshGenerator : IMeshGenerator
{
    public Mesh GenMeshCube(float width, float height, float length)
    {
        return Raylib.GenMeshCube(width, height, length);
    }

    public Mesh GenMeshSphere(float radius, int rings, int slices)
    {
        return Raylib.GenMeshSphere(radius, rings, slices);
    }

    public Mesh GenMeshHemiSphere(float radius, int rings, int slices)
    {
        return Raylib.GenMeshHemiSphere(radius, rings, slices);
    }

    public Mesh GenMeshCylinder(float radius, float height, int slices)
    {
        return Raylib.GenMeshCylinder(radius, height, slices);
    }
}
