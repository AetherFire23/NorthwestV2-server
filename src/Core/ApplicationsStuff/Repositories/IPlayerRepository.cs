using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.ApplicationsStuff.Repositories;

public interface IPlayerRepository
{
    public Task<List<Player>> GetPlayersInSameRoom(Guid playerId);
    public Task<List<Player>> GetOtherPlayersInGame(Guid playerId);
    public Task<Player> GetPlayer(Guid playerid);
    public Task<Player> GetPlayerWithRoomAndInventory(Guid playerId);
    public Task<List<Player>> GetPlayersInSameGame(Guid playerId);

    public void Add(Player player);
    public void AddRange(IEnumerable<Player> player);
    public Task<Player> GetPlayerAndRoomAndInventoryAndGame(Guid playerId);
}