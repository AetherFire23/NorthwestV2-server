using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Integration.Scratches;

namespace NorthwestV2.Integration.Helpers;

public static class PlayerHelper
{
    public static async Task<Guid> TeleportPlayerTo(this TestBase2 current, Guid playerId,
        RoomEnum roomenum)
    {
        Player player = await current.Scope.ServiceProvider.GetRequiredService<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);
        player.Inventory.Items.Add(new Scrap());
        Room room = await current.Context.Rooms.Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == roomenum);
        player.Room = room;
        await current.Context.SaveChangesAsync();
        return playerId;
    }
}