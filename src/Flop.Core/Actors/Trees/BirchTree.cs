using System.Numerics;

namespace Flop.Core.Actors.Trees;

/// <summary>
/// A birch tree.
/// This is the simplest tree in the game to cut down.
/// </summary>
public class BirchTree(Identity identity, Vector3 position, Quaternion rotation)
    : TreeBase(identity, position, rotation)
{
    public override float WoodcuttingDifficulty => 1.0f;
    public override int RequiredWoodcuttingLevel => 1;
    public override float LeafSphereRadius => 1.5f;
    public override float TrunkRadius => 0.6f;
    public override float TrunkHeight => 2.0f;
    public override Material TrunkMaterial => new(Color.Brown_800);
    public override Material LeafMaterial => new(Color.Green_700);

    public override string Description =>
        "Slender and pale, the birch is quick to fell and common across the lowlands. "
        + "Its soft wood isn't worth much, but it's perfect for beginners learning the axe.";
}
