using AetherFire23.ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.Repositories;

namespace NorthwestV2.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly NorthwestContext _northwestContext;

    public RoomRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

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
    public async ValueTask SaveRoomAndAdjacents(IEnumerable<Room> rooms)
    {
        Dictionary<Room, List<Room>> roomsToAdjacents = new Dictionary<Room, List<Room>>();
        foreach (Room room in rooms)
        {
            roomsToAdjacents.Add(room, new(room.AdjacentRooms));

            _northwestContext.Rooms.Add(room);
        }

        foreach (Room room in rooms)
        {
            room.AdjacentRooms.Clear();
        }

        // Saving first so that ef core can know about the existing rooms before trying to add cyclically 
        // nested entities. 
        await _northwestContext.SaveChangesAsync();

        foreach (var roomsToAdjacent in roomsToAdjacents)
        {
            roomsToAdjacent.Key.AdjacentRooms.AddRange(roomsToAdjacent.Value);
        }
    }

    public async Task<List<Room>> GetAdjacentRoomsOfPlayer(Guid playerId)
    {
        Player player = await this._northwestContext.Players
            .Include(x => x.Room)
            .ThenInclude(x => x.AdjacentRooms)
            .FirstAsync(x => x.Id == playerId);

        List<Room> adjacents = player.Room.AdjacentRooms;

        return adjacents;
    }
}