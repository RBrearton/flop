using System.Numerics;
using Flop.Core.Geometry;
using Raylib_cs;

namespace Flop.Client.Rendering;

/// <summary>
/// Main rendering coordinator.
/// Manages mesh and material caching, builds render batches from geometry rigs, and executes
/// instanced rendering.
/// </summary>
public class Renderer(MeshManager meshManager, MaterialManager materialManager) : IDisposable
{
    private readonly MeshManager _meshManager = meshManager;
    private readonly MaterialManager _materialManager = materialManager;
    private readonly RenderBatchCollection _batchCollection = new();

    /// <summary>
    /// Render all geometry rigs using the given camera.
    /// Builds batches, acquires meshes/materials, and renders via DrawMeshInstanced.
    /// </summary>
    public void Render(IEnumerable<IGeometryRig> rigs, Camera3D camera)
    {
        // Clear previous frame's batches.
        _batchCollection.Clear();

        // Build batches from all rigs.
        foreach (var rig in rigs)
        {
            BuildBatches(rig);
        }

        // Begin 3D rendering.
        Raylib.BeginMode3D(camera);

        // Render all batches.
        foreach (var batch in _batchCollection.GetBatches())
        {
            // Get or create mesh and material from managers.
            var mesh = GetMeshFromHandle(batch.MeshHandle);
            var material = GetMaterialFromHandle(batch.MaterialHandle);

            // Render the batch
            batch.Draw(mesh, material);
        }

        Raylib.EndMode3D();
    }

    /// <summary>
    /// Build render batches from a geometry rig.
    /// Calculates world transforms for all primitives and adds them to batches.
    /// </summary>
    private void BuildBatches(IGeometryRig rig)
    {
        // Rig world transform.
        var rigWorldTransform = CreateTransformMatrix(rig.Position, rig.Rotation);

        foreach (var component in rig.Components)
        {
            // Component local transform.
            var componentLocalTransform = CreateTransformMatrix(
                component.LocalPosition,
                component.LocalRotation
            );

            foreach (var primitive in component.Primitives)
            {
                // Primitive local transform.
                var primitiveLocalTransform = CreateTransformMatrix(
                    primitive.LocalPosition,
                    primitive.LocalRotation
                );

                // Compose final world transform: Rig * Component * Primitive.
                var worldTransform =
                    primitiveLocalTransform * componentLocalTransform * rigWorldTransform;

                _batchCollection.AddPrimitive(primitive, worldTransform);
            }
        }
    }

    /// <summary>
    /// Create a transform matrix from position and rotation.
    /// </summary>
    private static Matrix4x4 CreateTransformMatrix(Vector3 position, Quaternion rotation)
    {
        return Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
    }

    /// <summary>
    /// Get a mesh from the mesh manager. Creates and uploads if necessary.
    /// Note: This is a temporary solution. Ideally we'd acquire meshes upfront
    /// and track their lifecycle properly.
    /// </summary>
    private Mesh GetMeshFromHandle(MeshHandle handle)
    {
        // This is a simplified approach - we need to find the primitive associated with this
        // handle.
        // For now, this is a placeholder that will be addressed when we integrate with actual
        // actors.
        throw new NotImplementedException(
            "GetMeshFromHandle requires primitive lookup. "
                + "This will be implemented when integrating with actual actor system."
        );
    }

    /// <summary>
    /// Get a material from the material manager. Creates and loads if necessary.
    /// Note: This is a temporary solution. Ideally we'd acquire materials upfront
    /// and track their lifecycle properly.
    /// </summary>
    private Material GetMaterialFromHandle(MaterialHandle handle)
    {
        // This is a simplified approach - we need to find the material associated with this handle.
        // For now, this is a placeholder that will be addressed when we integrate with actual
        // actors.
        throw new NotImplementedException(
            "GetMaterialFromHandle requires material lookup. "
                + "This will be implemented when integrating with actual actor system."
        );
    }

    /// <summary>
    /// Clear all cached meshes and materials.
    /// Useful for cleanup between scenes.
    /// </summary>
    public void ClearCache()
    {
        _meshManager.Dispose();
        _materialManager.Dispose();
    }

    public void Dispose()
    {
        _meshManager.Dispose();
        _materialManager.Dispose();
        GC.SuppressFinalize(this);
    }
}
