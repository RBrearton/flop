using System.Numerics;
using Flop.Core.Geometry;

namespace Flop.Client.Rendering;

/// <summary>
/// Manages a collection of render batches, automatically grouping primitives
/// by their mesh and material handles for efficient instanced rendering.
/// </summary>
public class RenderBatchCollection
{
    private readonly Dictionary<(MeshHandle, MaterialHandle), RenderBatch> _batches = [];

    /// <summary>
    /// Add an instance to the collection with its mesh/material handles and world transform.
    /// Automatically groups it into the appropriate batch.
    /// </summary>
    public void AddInstance(
        MeshHandle meshHandle,
        MaterialHandle materialHandle,
        Matrix4x4 worldTransform
    )
    {
        var key = (meshHandle, materialHandle);

        if (!_batches.TryGetValue(key, out var batch))
        {
            batch = new RenderBatch(meshHandle, materialHandle);
            _batches[key] = batch;
        }

        batch.Add(worldTransform);
    }

    /// <summary>
    /// Get all batches in the collection.
    /// </summary>
    public IEnumerable<RenderBatch> GetBatches()
    {
        return _batches.Values;
    }

    /// <summary>
    /// Clear all batches, removing all transforms.
    /// Batches themselves are reused.
    /// </summary>
    public void Clear()
    {
        foreach (var batch in _batches.Values)
        {
            batch.Clear();
        }
    }

    /// <summary>
    /// Get the total number of batches (unique mesh/material combinations).
    /// </summary>
    public int BatchCount => _batches.Count;

    /// <summary>
    /// Get the total number of instances across all batches.
    /// </summary>
    public int TotalInstanceCount
    {
        get
        {
            int total = 0;
            foreach (var batch in _batches.Values)
            {
                total += batch.Transforms.Count;
            }
            return total;
        }
    }
}
