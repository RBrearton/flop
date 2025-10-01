namespace Flop.Core.Geometry;

/// <summary>
/// A handle that uniquely identifies a mesh based on its primitive parameters.
/// Two primitives with identical parameters will produce the same handle,
/// enabling mesh deduplication and GPU caching.
/// </summary>
public readonly record struct MeshHandle(int Hash)
{
    public MeshHandle(IGeometryPrimitive primitive)
        : this(
            primitive switch
            {
                Box box => new MeshHandle(box).Hash,
                Cylinder cylinder => new MeshHandle(cylinder).Hash,
                Sphere sphere => new MeshHandle(sphere).Hash,
                Hemisphere hemisphere => new MeshHandle(hemisphere).Hash,
                _
                    => throw new ArgumentException(
                        $"Unknown primitive type: {primitive.GetType().Name}"
                    ),
            }
        ) { }

    public MeshHandle(Box box)
        : this(HashCode.Combine(nameof(Box), box.Size.X, box.Size.Y, box.Size.Z)) { }

    public MeshHandle(Cylinder cylinder)
        : this(
            HashCode.Combine(nameof(Cylinder), cylinder.Radius, cylinder.Height, cylinder.Slices)
        ) { }

    public MeshHandle(Sphere sphere)
        : this(HashCode.Combine(nameof(Sphere), sphere.Radius, sphere.Rings, sphere.Slices)) { }

    public MeshHandle(Hemisphere hemisphere)
        : this(
            HashCode.Combine(
                nameof(Hemisphere),
                hemisphere.Radius,
                hemisphere.Rings,
                hemisphere.Slices
            )
        ) { }
}
