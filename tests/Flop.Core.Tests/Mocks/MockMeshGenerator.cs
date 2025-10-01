using Flop.Core.Geometry;
using Raylib_cs;

namespace Flop.Core.Tests.Mocks;

/// <summary>
/// Mock implementation of IMeshGenerator for testing.
/// Generates dummy meshes without raylib initialization.
/// </summary>
public class MockMeshGenerator : IMeshGenerator
{
    public int CubeCount { get; private set; }
    public int SphereCount { get; private set; }
    public int HemiSphereCount { get; private set; }
    public int CylinderCount { get; private set; }

    public Mesh GenMeshCube(float width, float height, float length)
    {
        CubeCount++;
        return new Mesh(); // Dummy mesh
    }

    public Mesh GenMeshSphere(float radius, int rings, int slices)
    {
        SphereCount++;
        return new Mesh(); // Dummy mesh
    }

    public Mesh GenMeshHemiSphere(float radius, int rings, int slices)
    {
        HemiSphereCount++;
        return new Mesh(); // Dummy mesh
    }

    public Mesh GenMeshCylinder(float radius, float height, int slices)
    {
        CylinderCount++;
        return new Mesh(); // Dummy mesh
    }
}
