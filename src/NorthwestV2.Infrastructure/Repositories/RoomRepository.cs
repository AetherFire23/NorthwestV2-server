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
        Player player = await this._northwestContext.Players
            .Include(x => x.Room)
            .ThenInclude(x => x.Connections)
            .ThenInclude(x => x.Room1)
            .ThenInclude(x => x.Connections)
            .ThenInclude(x => x.Room2)
            .FirstAsync(x => x.Id == playerId);

        // TODO: Make Room Connection a pure Join table and query from there. 


        return new List<Room>();
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
}