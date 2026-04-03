using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Initiation;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.Productions;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions;

[TestSubject(typeof(CancelProductionActionApp))]
public class CancelProductionActionAppTest : NorthwestIntegrationTestBase
{
    public CancelProductionActionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenUnfinishedSpyglassInRoom_WhenGettingAvailableActions_ThenIsCancelProductionAvailable()
    {
        Guid playerId = await SetupForSpyglassStartAction();

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        Assert.Contains(actions.Actions, x => x.Name == ActionNames.CancelProduction);
    }

    [Fact]
    public async Task GivenUnfinishedSpyglassInRoom_WhenGettingAvailableActions_ThenCanCancelProduction()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        // Create the spyglass
        await this.Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
            ActionTargets = []
        });

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        ActionDto requirements = actions.Actions.First(x => x.Name == ActionNames.CancelProduction);
        Assert.True(requirements.Requirements.All(x => x.IsFulfilled));
    }

    [Fact]
    public async Task GivenUnfinishedSpyglassInRoom_WhenCancellingProduction_ThenProductionItemIsDeleted()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        // Create the spyglass
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
            ActionTargets = []
        });
        
        // Cancel the unfinished item 
        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });
        var akt = actions.Actions.First(x => x.Name == ActionNames.CancelProduction);
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CancelProduction,
            PlayerId = playerId,
            ActionTargets = [[akt.Prompts.First().ValidTargets.First()]]
        });


        this._scope = base.RootServiceProvider.CreateScope();
        Player player2 = await GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);

        Assert.True(!player2.Room.Inventory.Items.Any(x => x.ItemType == ItemTypes.UnfinishedSpyglass));
    }

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
        Room room = await Context.Rooms.Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == roomenum);
        player.Room = room;
        await Context.SaveChangesAsync();
        return playerId;
    }
}