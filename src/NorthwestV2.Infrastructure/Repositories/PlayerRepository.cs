using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.EfCoreExtensions;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;

namespace NorthwestV2.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly NorthwestContext _northwestContext;

    public PlayerRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    private Expression<Func<Player, bool>> GameIdScoped(Player player)
    {
        return (other) => other.GameId == player.GameId;
    }

    public async Task<List<Player>> GetPlayersInSameRoom(Guid playerId)
    {
        Player player = await GetPlayer(playerId);

        List<Player> playersInGame = _northwestContext.Players
            .Where(GameIdScoped(player))
            .Where(x => x.RoomId == player.RoomId && x.Id != playerId)
            .ToList();

        return playersInGame;
    }

    public async Task<List<Player>> GetOtherPlayersInGame(Guid playerId)
    {
        Player player = await GetPlayer(playerId);
        var otherPlayers = await _northwestContext.Players
            .Where(GameIdScoped(player))
            .Where(x => x.Id != player.Id)
            .ToListAsync();

        return otherPlayers;
    }

    public async Task<Player> GetPlayer(Guid playerid)
    {
        Player user = await _northwestContext.Players
            .FindById(playerid);
        return user;
    }

    public async Task<Player> GetPlayerWithRoomAndInventory(Guid playerId)
    {
        Player user = await _northwestContext.Players
            .Include(x => x.Room)
            .ThenInclude(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .FirstAsync(x => x.Id == playerId);

        return user;
    }

    public async Task<Player> GetPlayerAndRoomAndInventoryAndGame(Guid playerId)
    {
        Player user = await _northwestContext.Players
            .Include(x => x.Room)
            .ThenInclude(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .Include(x => x.Game)
            .ThenInclude(x => x.Rooms)
            .ThenInclude(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .FirstAsync(x => x.Id == playerId);

        return user;
    }

    public async Task<List<Player>> GetPlayersInSameGame(Guid playerId)
    {
        Player player = await _northwestContext.Players.FindById(playerId);

        List<Player> players = await _northwestContext.Players
            .Where(GameIdScoped(player))
            .ToListAsync();

        return players;
    }

    public void Add(Player player)
    {
        _northwestContext.Players.Add(player);
    }

    public void AddRange(IEnumerable<Player> player)
    {
        _northwestContext.Players.AddRange(player);
    }
}