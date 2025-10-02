using Flop.Core.Geometry;

namespace Flop.Client;

/// <summary>
/// A client-side wrapper for IGeometryRig that maps primitives to rendering materials.
/// Keeps core geometry logic separate from rendering concerns.
/// </summary>
/// <remarks>
/// Create a renderable rig with a material mapping.
/// </remarks>
/// <param name="rig">The core geometry rig.</param>
/// <param name="materialMap">Mapping from mesh handles to material handles.</param>
public class RenderableRig(IGeometryRig rig, Dictionary<MeshHandle, MaterialHandle> materialMap)
{
    private readonly IGeometryRig _rig = rig;
    private readonly Dictionary<MeshHandle, MaterialHandle> _materialMap = materialMap;

    /// <summary>
    /// The underlying geometry rig.
    /// </summary>
    public IGeometryRig Rig => _rig;

    /// <summary>
    /// Get all render instances for this rig.
    /// Each instance contains mesh, material, and local transform information.
    /// </summary>
    /// <returns>Enumerable of render instances.</returns>
    public IEnumerable<RenderInstance> GetRenderInstances()
    {
        foreach (var primitive in _rig.AllPrimitives())
        {
            MeshHandle meshHandle = new(primitive);
            if (_materialMap.TryGetValue(meshHandle, out MaterialHandle material))
            {
                yield return new RenderInstance(
                    meshHandle,
                    material,
                    primitive.LocalPosition,
                    primitive.LocalRotation
                );
            }
        }
    }
}
