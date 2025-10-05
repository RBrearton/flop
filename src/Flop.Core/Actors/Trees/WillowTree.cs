using System.Numerics;

namespace Flop.Core.Actors.Trees;

/// <summary>
/// A willow tree.
/// Slightly tougher than a birch, with drooping leaves and flexible wood.
/// </summary>
public class WillowTree(Identity identity, Vector3 position, Quaternion rotation)
    : TreeBase(identity, position, rotation)
{
    public override float WoodcuttingDifficulty => 1.5f;
    public override int RequiredWoodcuttingLevel => 5;
    public override float LeafSphereRadius => 2.0f;
    public override float TrunkRadius => 0.7f;
    public override float TrunkHeight => 2.5f;
    public override Material TrunkMaterial => new(Color.Brown_700);
    public override Material LeafMaterial => new(Color.Green_600);

    public override string Description =>
        "Graceful and long-limbed, the willow grows near rivers and lakes. "
        + "Its wood is supple and slightly tougher to cut than birch, "
        + "making it a good test for novice woodcutters.";
}
