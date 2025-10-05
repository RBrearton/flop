namespace Flop.Core.Spatial;

/// <summary>
/// This class contains extension methods for the Flop.Core.Spatial namespace.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Returns the cell at which this actor lives. Useful for interfacing with SpatialHashGrids.
    /// </summary>
    /// <param name="cellLength">The side length of each cell.</param>
    /// <returns>A value tuple containing the cell in which our actor lives.</returns>
    public static (int x, int z) GetCell(this Actor actor, float cellLength)
    {
        // A simple enough algorithm. Take the current position, divide it by the cell length,
        // and return the tuple of integers.
        int x = (int)MathF.Floor(actor.Position.X / cellLength);
        int z = (int)MathF.Floor(actor.Position.Z / cellLength);

        return (x, z);
    }

    public static void AddActor<T>(
        this Dictionary<(int x, int z), List<T>> cellToActorsDict,
        T actor,
        float cellLength
    )
        where T : Actor
    {
        // First, we need to see if the actor's cell exists in the dictionary.
        bool cellExists = cellToActorsDict.TryGetValue(
            actor.GetCell(cellLength),
            out List<T>? actorList
        );

        // If the cell doesn't exist, we need to add a new key, value pair to the dictionary.
        // The key is the cell, and the value is a list containing only our actor.
        if (!cellExists)
        {
            cellToActorsDict.Add(actor.GetCell(cellLength), [actor]);
            return;
        }

        // If, for some reason, the cell exists but the actor list is null, we need to replace
        // the null reference with a list containing only our actor.
        if (actorList is null)
        {
            cellToActorsDict[actor.GetCell(cellLength)] = [actor];
            return;
        }

        // If the cell exists and the actorList isn't null, then we should just add our actor to the
        // list of actors in that cell.
        if (cellExists)
        {
            actorList.Add(actor);
        }
    }
}
