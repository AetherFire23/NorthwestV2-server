using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.SpyglassProduction;

[TestSubject(typeof(SpyglassStartProductionApp))]
public class SpyglassStartProductionAppTest : NorthwestIntegrationTestBase
{
    public SpyglassStartProductionAppTest(ITestOutputHelper output) : base(output)
    {
    }
    /*
     * 1st stage tests
     */

    /*
     * Item can be owend by room OR player (not a problem usuers)
     * But now I need 2 fks, doesnt work.
     */

    [Fact]
    public async Task GivenPlayerWithScrapInInventory_WhenGetProductionAvailability_ThenIsScrapInRoom()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = gameDataSeed.PlayerIds.First();
        Player player = await _scope.ServiceProvider.GetRequiredService<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);
        player.Inventory.Items.Add(new Scrap());
        // TODO: Teleport player to the required room. 
        var room = await Context.Rooms.Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == RoomEnum.Armory);
        player.Room = room;
        await Context.SaveChangesAsync();
        _scope = this.RootServiceProvider.CreateScope();
        player = await _scope.ServiceProvider.GetRequiredService<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);
        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        Assert.True(actions.Actions.First(x => x.Name == ActionNames.SpyglassProductionStart).Requirements
            .All(x => x.IsFulfilled));
        
        int i = 0;
    }

    private async Task PlaceScrapInRoom()
    {
    }

    // todo: Test that we can execute the first stage
}