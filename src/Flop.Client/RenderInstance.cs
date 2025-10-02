using System.Numerics;
using Flop.Core.Geometry;

namespace Flop.Client;

/// <summary>
/// Represents a single instance to be rendered.
/// Contains the mesh, material, and local transform for one primitive.
/// </summary>
public readonly record struct RenderInstance(
    MeshHandle Mesh,
    MaterialHandle Material,
    Vector3 LocalPosition,
    Quaternion LocalRotation
);
