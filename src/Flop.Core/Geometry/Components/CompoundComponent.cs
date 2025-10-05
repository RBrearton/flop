using System.Numerics;

namespace Flop.Core.Geometry.Components;

/// <summary>
/// A CompoundComponent is a geometry component wrapping a collection of primitives.
/// The component has a local position and rotation, which is applied to all primitives on top of
/// their own local position/rotation.
/// </summary>
public class CompoundComponent(
    IReadOnlyList<IGeometryPrimitive> primitives,
    Vector3 localPosition,
    Quaternion localRotation
) : IGeometryComponent
{
    public IReadOnlyList<IGeometryPrimitive> Primitives => primitives;

    public AxisAlignedBoundingBox BoundingBox
    {
        get
        {
            if (primitives.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot create AABB for empty compound component"
                );
            }

            // Union all primitive AABBs in component-local space.
            var localAABB = primitives
                .Select(p => p.BoundingBox)
                .Aggregate(AxisAlignedBoundingBox.Union);

            // If no rotation, just translate the AABB
            if (LocalRotation == Quaternion.Identity)
            {
                return new AxisAlignedBoundingBox(
                    localAABB.Min + LocalPosition,
                    localAABB.Max + LocalPosition
                );
            }

            // Transform the 8 corners of the local AABB by component rotation+position.
            var corners = new[]
            {
                localAABB.Min,
                new Vector3(localAABB.Min.X, localAABB.Min.Y, localAABB.Max.Z),
                new Vector3(localAABB.Min.X, localAABB.Max.Y, localAABB.Min.Z),
                new Vector3(localAABB.Min.X, localAABB.Max.Y, localAABB.Max.Z),
                new Vector3(localAABB.Max.X, localAABB.Min.Y, localAABB.Min.Z),
                new Vector3(localAABB.Max.X, localAABB.Min.Y, localAABB.Max.Z),
                new Vector3(localAABB.Max.X, localAABB.Max.Y, localAABB.Min.Z),
                localAABB.Max,
            };
            var transformedCorners = corners.Select(c =>
                Vector3.Transform(c, LocalRotation) + LocalPosition
            );

            // Get the AABB of the transformed corners.
            return AxisAlignedBoundingBox.FromPoints(transformedCorners);
        }
    }

    public Vector3 LocalPosition => localPosition;
    public Quaternion LocalRotation => localRotation;
}
