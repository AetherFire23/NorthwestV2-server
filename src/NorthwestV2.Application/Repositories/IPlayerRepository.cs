using AetherFire23.ERP.Domain.Entity;

namespace NorthwestV2.Application.Repositories;

public interface IPlayerRepository
{
    public Task<List<Player>> GetPlayersInSameRoom(Guid playerId);
    public Task<List<Player>> GetOtherPlayersInGame(Guid playerId);
    public Task<Player> GetPlayer(Guid playerid);

    public Task<List<Player>> GetPlayersInSameGame(Guid playerId);

    public void Add(Player player);
    public void AddRange(IEnumerable<Player> player);
}