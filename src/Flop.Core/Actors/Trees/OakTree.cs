using System.Numerics;

namespace Flop.Core.Actors.Trees;

/// <summary>
/// An oak tree.
/// A sturdy, broad tree with dense wood prized by craftsmen.
/// </summary>
public class OakTree(Identity identity, Vector3 position, Quaternion rotation)
    : TreeBase(identity, position, rotation)
{
    public override float WoodcuttingDifficulty => 2.5f;
    public override int RequiredWoodcuttingLevel => 10;
    public override float LeafSphereRadius => 2.5f;
    public override float TrunkRadius => 0.9f;
    public override float TrunkHeight => 3.0f;
    public override Material TrunkMaterial => new(Color.Brown_600);
    public override Material LeafMaterial => new(Color.Green_500);

    public override string Description =>
        "Thick-trunked and enduring, the oak is a symbol of strength. "
        + "Its hard wood takes longer to cut, but yields strong, valuable timber "
        + "favoured by builders and artisans alike.";
}
