using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.ApplicationsStuff.Repositories;

public interface IRoomRepository
{


    public Task<List<Room>> GetAdjacentRoomsOfPlayer(Guid playerId);

    public Task<Room> GetRoomInPlayersGame(Player player, RoomEnum roomEnum);
    public Task<Room> GetRoomInGame(Game game, RoomEnum room);

    public Task SaveRooms(List<Room> rooms);
}