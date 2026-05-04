using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly NorthwestContext _northwestContext;

    public RoomRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public async Task SaveRooms(List<Room> rooms)
    {
        _northwestContext.Rooms.AddRange(rooms);
        await _northwestContext.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAdjacentRoomsOfPlayer(Guid playerId)
    {
        Player player = _northwestContext
            .Players
            .Include(x => x.Room)
            .First(x => x.Id == playerId);

        var currentRoomId = player.Room.Id;

        List<RoomConnection> adj = _northwestContext
            .RoomConnection
            .Where(x => x.Room1Id == currentRoomId || x.Room2Id == currentRoomId)
            .ToList();

        // TODO: Make Room Connection a pure Join table and query from there. 

        List<Room> adjacents = new List<Room>();
        foreach (var roomConnection in adj)
        {
            Guid otherRoomId = roomConnection.Room1Id == currentRoomId
                ? roomConnection.Room2Id
                : roomConnection.Room1Id;

            Room otherRoom = await _northwestContext.Rooms
                .Include(x => x.Inventory)
                .ThenInclude(x => x.Items)
                .Include(x => x.Game)
                .FirstAsync(x => x.Id == otherRoomId);

            adjacents.Add(otherRoom);
        }

        return adjacents;
    }

    /// <summary>
    /// Gets a room but scoped to a specific game ID.
    /// Either from the player's game ID or the Room's game ID. 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="room"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Room> GetRoomInPlayersGame(Player player, RoomEnum roomEnum)
    {
        Room room = await _northwestContext.Rooms
            .Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == roomEnum);

        return room;
    }

    public async Task<Room> GetRoomInGame(Game game, RoomEnum roomEnmum)
    {
        Room room = await _northwestContext.Rooms
            .Where(x => x.GameId == game.Id)
            .FirstAsync(x => x.RoomEnum == roomEnmum);

        return room;
    }

    public async Task SaveRoomConnections(List<RoomConnection> connections)
    {
        _northwestContext.RoomConnection.AddRange(connections);
        await _northwestContext.SaveChangesAsync();
    }
}