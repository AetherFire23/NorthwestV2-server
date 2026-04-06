using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Repositories;

public interface IRoomRepository
{
    /// <summary>
    /// Persists a collection of rooms and their adjacency relationships while avoiding
    /// EF Core cyclic navigation issues. This method temporarily removes adjacency links,
    /// saves the rooms, and then restores the adjacency graph.
    /// </summary>
    /// <param name="rooms">
    /// The collection of rooms whose adjacency relationships must be saved.
    /// </param>
    /// <remarks>
    /// EF Core cannot persist cyclic navigation graphs in a single operation without
    /// attempting to insert nested entities recursively.  
    /// 
    /// To work around this:
    /// <list type="number">
    /// <item>All rooms are added to the context.</item>
    /// <item>Their adjacency lists are temporarily cleared.</item>
    /// <item>The rooms are saved so EF Core assigns IDs and tracks them.</item>
    /// <item>The original adjacency relationships are restored.</item>
    /// </list>
    /// 
    /// This ensures that the room graph (which may contain cycles) is persisted safely
    /// without EF Core attempting to re‑insert already tracked entities.
    /// </remarks>
    public ValueTask SaveRoomAndAdjacents(IEnumerable<Room> rooms);

    public Task<List<Room>> GetAdjacentRoomsOfPlayer(Guid playerId);

    public Task<Room> GetRoomInPlayersGame(Player player, RoomEnum roomEnum);
    public Task<Room> GetRoomInGame(Game game, RoomEnum room);
}