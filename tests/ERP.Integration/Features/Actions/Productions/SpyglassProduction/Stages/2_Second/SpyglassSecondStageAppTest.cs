
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Initiation;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction.ContributionToStages._2_Second;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.SpyglassProduction.Stages._2_Second;

[TestSubject(typeof(SpyglassSecondStageApp))]
public class SpyglassSecondStageAppTest : NorthwestIntegrationTestBase
{
    public SpyglassSecondStageAppTest(ITestOutputHelper output) : base(output)
    {
    }

    // [Fact]
    // public async Task GivenUnfinishedSpyglassAndPlayerInCorrectRoom_WhenGetActions_ThenCanStartSecondStage()
    // {
    //     Guid playerId = await SetupForSpyglassStartAction();
    //     await Mediator.Send(new ExecuteActionRequest()
    //     {
    //         ActionName = ActionNames.SpyglassProductionStart,
    //         PlayerId = playerId,
    //     });
    //     Assert.Fail();
    // }
    //
    // [Fact]
    // public void GivenUnfinishedSpyGlass_WhenContributingForTheFirstTIme_ThenPointsAreAdded()
    // {
    //     Assert.Fail();
    // }
    
    private async Task<Guid> SetupForSpyglassStartAction()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = await TeleportPlayerTo(gameDataSeed,
            SpyglassProductionInitiationAction.REQUIRED_ROOM_SPYGLASS_START);
        return playerId;
    }

    private async Task<Guid> TeleportPlayerTo(GameDataSeed gameDataSeed, RoomEnum roomenum)
    {
        Guid playerId = gameDataSeed.PlayerIds.First();
        Player player = await _scope.ServiceProvider.GetRequiredService<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);
        player.Inventory.Items.Add(new Scrap());
        //  Teleport player to the required room. 
        var room = await Context.Rooms.Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == roomenum);
        player.Room = room;
        await Context.SaveChangesAsync();
        return playerId;
    }
}