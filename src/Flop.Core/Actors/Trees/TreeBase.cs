using System.Numerics;
using Flop.Core.Geometry;
using Flop.Core.Geometry.Components;
using Flop.Core.Geometry.Primitives;
using Flop.Core.Geometry.Rigs;

namespace Flop.Core.Actors.Trees;

/// <summary>
/// The base class for all trees.
/// </summary>
public abstract class TreeBase(Identity identity, Vector3 position, Quaternion rotation)
    : EnvironmentActorBase(identity, position, rotation)
{
    /// <summary>
    /// The difficulty associated with cutting down this tree.
    /// </summary>
    public abstract float WoodcuttingDifficulty { get; }

    /// <summary>
    /// The woodcutting level required to attempt to cut down this tree.
    /// </summary>
    public abstract int RequiredWoodcuttingLevel { get; }

    /// <summary>
    /// The radius of the tree's leaf sphere.
    /// </summary>
    public abstract float LeafSphereRadius { get; }

    /// <summary>
    /// The radius of the tree's trunk.
    /// </summary>
    public abstract float TrunkRadius { get; }

    /// <summary>
    /// The height of the tree's trunk.
    /// </summary>
    public abstract float TrunkHeight { get; }

    /// <summary>
    /// The trunk's material.
    /// </summary>
    public abstract Material TrunkMaterial { get; }

    /// <summary>
    /// The leaf's material.
    /// </summary>
    public abstract Material LeafMaterial { get; }

    public override IGeometryRig GeometryRig =>
        new CompoundComponent([Trunk, Leaves,]).AsStaticRig();

    /// <summary>
    /// The cylinder primitive representing the trunk of the tree.
    /// </summary>
    public Cylinder Trunk =>
        new(TrunkRadius, TrunkHeight, TrunkMaterial, localPosition: TrunkTranslation);

    /// <summary>
    /// The sphere primitive representing the leaves of the tree.
    /// </summary>
    public Sphere Leaves => new(LeafSphereRadius, LeafMaterial, localPosition: LeavesTranslation);

    /// <summary>
    /// The vector by which we need to translate the trunk so that its base lies on the y=0 plane.
    /// </summary>
    private Vector3 TrunkTranslation => new(0, TrunkHeight / 2, 0);

    /// <summary>
    /// The vector by which we need to translate the leaves so that the base of the sphere lies
    /// exactly on top of the trunk.
    /// </summary>
    private Vector3 LeavesTranslation => new(0, TrunkHeight + LeafSphereRadius, 0);
}
