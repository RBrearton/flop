using System.Numerics;
using Flop.Core;
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
    /// Render all renderables using the given camera.
    /// Builds batches, acquires meshes/materials, and renders via DrawMeshInstanced.
    /// </summary>
    public void Render(IEnumerable<Flop.Core.IRenderable> renderables, Camera3D camera)
    {
        // Clear previous frame's batches.
        _batchCollection.Clear();

        // Build batches from all renderables.
        foreach (var renderable in renderables)
        {
            BuildBatches(renderable);
        }

        // Begin 3D rendering.
        Raylib.BeginMode3D(camera);

        // Render all batches.
        foreach (var batch in _batchCollection.GetBatches())
        {
            // Get mesh and material from managers using handles.
            var mesh = _meshManager.GetMesh(batch.MeshHandle);
            var material = _materialManager.GetMaterial(batch.MaterialHandle);

            // Render the batch
            batch.Draw(mesh, material);
        }

        Raylib.EndMode3D();
    }

    /// <summary>
    /// Build render batches from a renderable.
    /// Calculates world transforms for all primitives and adds them to batches.
    /// </summary>
    private void BuildBatches(Flop.Core.IRenderable renderable)
    {
        // Renderable world transform.
        var rigWorldTransform = CreateTransformMatrix(
            ((IHasPlacement)renderable).Position,
            ((IHasPlacement)renderable).Rotation
        );

        foreach (var component in renderable.Components)
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

                // Compute handles for batching (primitives should already be uploaded via World.AddActor).
                var meshHash = MeshManager.ComputeHash(primitive);
                var materialHash = MaterialManager.ComputeHash(primitive.Material);
                var meshHandle = MeshHandle.FromHashCode(meshHash);
                var materialHandle = MaterialHandle.FromHashCode(materialHash);

                _batchCollection.AddInstance(meshHandle, materialHandle, worldTransform);
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
