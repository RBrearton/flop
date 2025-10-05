namespace Flop.Core.Spatial;

/// <summary>
/// An implementation of a simple spatial hash grid.
/// This is probably the simplest computationally efficient way to store and query objects in ND
/// where N is 3 or less - it starts scaling pretty badly in 4D+.
/// It's particularly useful when making 2D queries, which is the kind of queries we're most
/// interested in here.
///
/// When making spatial queries over a characteristic length-scale that you know in advance, this
/// can be a fantastically efficient way to store and query objects - set the CellLength to be
/// slightly larger than the characteristic length-scale of the range queries you're interested in,
/// and this will be about as good as it gets.
/// For more advanced queries, it can become a bit limiting.
///
/// This algorithm works by dividing the world into a grid of cells, and then storing the actors in
/// the cells that they occupy.
/// When trying to find nearest actors near a certain position, we just need to look in the cell
/// containing that position, as well as all adjacent cells.
/// </summary>
/// <typeparam name="T">The type of actor to store in the SpatialHashGrid.</typeparam>
public class SpatialHashGrid<T>
    where T : Actor
{
    // The side length of each cell in our SpatialHashGrid.
    public float CellLength { get; protected set; }

    readonly Dictionary<(int x, int z), List<T>> _cellToActors;
    readonly Dictionary<Identity, (int x, int z)> _actorNameToCell;

    public SpatialHashGrid(List<T> actors, float cellLength)
    {
        // Store the length of each cell.
        CellLength = cellLength;

        // Initialize our dictionaries.
        _cellToActors = [];
        _actorNameToCell = [];

        // We want to populate the _cellToActors lookup and the _actorNameToCell reverse lookup.
        foreach (var actor in actors)
        {
            _cellToActors.AddActor(actor, cellLength);
            _actorNameToCell[actor.Identity] = actor.GetCell(cellLength);
        }
    }

    /// <summary>
    /// Adds the actor passed as an argument to this SpatialHashGrid.
    /// </summary>
    /// <param name="actor">The actor to be added to the SpatialHashGrid.</param>
    public void AddActor(T actor)
    {
        _cellToActors.AddActor(actor, CellLength);
        _actorNameToCell[actor.Identity] = actor.GetCell(CellLength);
    }

    /// <summary>
    /// Removes the Actor passed as an argument from the SpatialHashGrid.
    /// </summary>
    /// <param name="actor">The actor we want to remove from the SpatialHashGrid.</param>
    public void RemoveActor(T actor)
    {
        // Find the actor's cell using the _actorNameToCell lookup table, while removing our
        // actor's entry from the _actorNameToCell dictionary.
        _actorNameToCell.Remove(actor.Identity, out var actorCell);

        // Remove the actor's entry from the _cellToActors dictionary.
        _cellToActors.Remove(actorCell);
    }

    /// <summary>
    /// Removes an actor from the SpatialHashGrid, given the actor's name.
    /// </summary>
    /// <param name="actorName">The name of the actor that we want to remove.</param>
    public void RemoveActor(Identity actorIdentity)
    {
        // Find the actor's cell using the _actorNameToCell lookup table, while removing our
        // actor's entry from the _actorNameToCell dictionary.
        _actorNameToCell.Remove(actorIdentity, out var actorCell);

        // Remove the actor's entry from the _cellToActors dictionary.
        _cellToActors.Remove(actorCell);
    }

    /// <summary>
    /// Returns a list of all actors in this SpatialHashGrid.
    /// </summary>
    public List<T> GetAll()
    {
        return _cellToActors.Values.SelectMany(x => x).ToList();
    }

    /// <summary>
    /// Tries to return an Actor object, given the identity of the actor.
    /// If no actor with this identity is found in this SpatialHashGrid, a null reference will be
    /// returned.
    /// </summary>
    /// <param name="identity">The identity of the actor we're searching for.</param>
    /// <returns>
    /// Either the Actor, or null, depending on whether the Actor could be found.
    /// </returns>
    public T? GetActorFromIdentity(Identity identity)
    {
        // Try to get the cell of the actor.
        bool couldFindActor = _actorNameToCell.TryGetValue(identity, out var actorCell);
        if (couldFindActor)
        {
            // Grab the list of actors from the _cellToActors dict.
            var actorList = _cellToActors[actorCell];

            // Find the actor in the list, and return the actor.
            foreach (var actor in actorList)
                if (actor.Identity == identity)
                    return actor;
        }

        // If execution reaches here, we failed to find the actor in this SpatialHashGrid.
        return null;
    }

    /// <summary>
    /// Tries to update the position of the actor in this SpatialHashGrid. Returns the truthiness
    /// of whether we had to update the actor's position. This method should be called whenever
    /// an actor changes position.
    /// </summary>
    /// <param name="actor">The actor that changed position.</param>
    /// <returns>Whether we needed to change the actor's cell in the SpatialHashGrid.</returns>
    public bool TryUpdateActorPosition(T actor)
    {
        // Work out where the actor is currently stored.
        var currentCell = GetActorsCurrentCell(actor);

        // Ask the actor where it really is.
        var correctCell = actor.GetCell(CellLength);

        // If these cells aren't correct, then we need to fully update the actor.
        if (currentCell != correctCell)
        {
            // The simplest way to do this is to completely remove the actor, then re-add the actor.
            RemoveActor(actor);
            AddActor(actor);
            return true;
        }

        // If execution reaches here, we didn't need to update the actor's position, in which
        // case we return False.
        return false;
    }

    /// <summary>
    /// The method that justifies the existence of this data structure!
    /// This method returns a list of all actors that are close to the actor that is passed as an
    /// argument.
    /// This can be used to quickly lookup everything that we need to send to a client.
    /// The list returned by this method does not include the actor passed as an argument.
    /// </summary>
    /// <param name="actor">The actor that we want to find other actors near.</param>
    /// <returns>
    /// All of the actors near the actor passed as an argument.
    /// This list does not include the actor passed as an argument.
    /// </returns>
    public List<T> GetActorsNear(Actor actor)
    {
        // First we need to grab the list of all cells near a certain cell.
        var nearbyCells = GetCellsNearCell(actor.GetCell(CellLength));

        // Now iterate over all nearby cells and grab every actor.
        List<T> nearbyActors = [];
        foreach (var cell in nearbyCells)
        {
            // Get all the actors in this cell.
            var actorsInCell = GetActorsInCell(cell);

            // Make sure that we don't add the actor that was passed as an argument!
            foreach (var actorInCell in actorsInCell)
                if (actorInCell.Identity != actor.Identity)
                    nearbyActors.Add(actorInCell);
        }

        // Return our freshly populated list of actors.
        return nearbyActors;
    }

    /// <summary>
    /// Returns the cell that the actor is currently associated with in this SpatialHashGrid. This
    /// is trivial using dictionary access, but methods are more readable!
    /// </summary>
    /// <param name="actor">The actor whose cell we're interested in.</param>
    /// <returns>
    /// The cell that the actor is currently associated with in this SpatialHashGrid.
    /// </returns>
    private (int x, int z) GetActorsCurrentCell(T actor)
    {
        return _actorNameToCell[actor.Identity];
    }

    /// <summary>
    /// Returns the list of all of the actors in the cell. If the cell is empty, returns an
    /// empty list. This method is safer than accessing the protected dictionary directly,
    /// because there is no guarantee that the dictionary has any particular cell key in it.
    /// </summary>
    /// <param name="cell">The cell to get the actors from.</param>
    /// <returns>The list of actors in the cell.</returns>
    private List<T> GetActorsInCell((int x, int z) cell)
    {
        // If the key doesn't exist, there clearly aren't any actors there; return an empty list.
        bool cellExists = _cellToActors.TryGetValue(cell, out List<T>? value);
        if (!cellExists)
            return [];

        // If the list is null, return an empty list.
        if (value is null)
            return [];

        // If the key exists, use normal dictionary access.
        return value;
    }

    /// <summary>
    /// Returns the list of all cells that are adjacent to the cell passed as an argument.
    /// </summary>
    /// <param name="cell">The cell to get the adjacent cells from.</param>
    /// <returns>The list of adjacent cells.</returns>
    private static List<(int x, int z)> GetCellsNearCell((int x, int z) cell)
    {
        int x = cell.x;
        int z = cell.z;

        // Sure, I could write an algorithm that works for ND, and I could write an
        // N-dimensional spatial hash grid, but... who cares. For a standard video game we only
        // ever need 2D lookups.
        return
        [
            (x + 1, z + 1),
            (x, z + 1),
            (x - 1, z + 1),
            (x + 1, z),
            (x, z),
            (x - 1, z),
            (x + 1, z - 1),
            (x, z - 1),
            (x - 1, z - 1),
        ];
    }
}
