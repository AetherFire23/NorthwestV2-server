using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Infrastructure;

namespace NorthwestV2.Integration;

public static class PlayerHelper
{
    public static async Task<Guid> TeleportPlayerTo(IServiceScope scope, NorthwestContext context,
        GameDataSeed gameDataSeed,
        RoomEnum roomenum)
    {
        Guid playerId = gameDataSeed.PlayerIds.First();
        Player player = await scope.ServiceProvider.GetRequiredService<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);
        player.Inventory.Items.Add(new Scrap());

        // scope rooms to those available only in the game of the player.
        Room room = await context.Rooms.Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == roomenum);

        // Set the player's room to that room. 
        player.Room = room;
        await context.SaveChangesAsync();
        return playerId;
    }
}