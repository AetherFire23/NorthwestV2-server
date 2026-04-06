using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Initiation;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.Productions;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions;

[TestSubject(typeof(CancelProductionActionApp))]
public class CancelProductionActionAppTest : TestBase2
{
    public CancelProductionActionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenUnfinishedSpyglassInRoom_WhenGettingAvailableActions_ThenCanCancelProduction()
    {
        Guid playerId = await SetupForSpyglassProduction();

        ActionDto cancelAction = await GetCancelProductionAction(playerId);

        Assert.True(cancelAction.Requirements.All(x => x.IsFulfilled));
    }

    [Fact]
    public async Task GivenUnfinishedSpyglassInRoom_WhenGettingAvailableActions_ThenIsCancelProductionAvailable()
    {
        Guid playerId = await SetupForSpyglassProduction();

        ActionDto cancelAction = await GetCancelProductionAction(playerId);

        Assert.NotNull(cancelAction);
    }

    [Fact]
    public async Task GivenUnfinishedSpyglassInRoom_WhenCancellingProduction_ThenProductionItemIsDeleted()
    {
        Guid playerId = await SetupForSpyglassProduction();
        ActionTarget productionTarget = await GetProductionTarget(playerId);

        await CancelProduction(playerId, productionTarget);

        await AssertNoProductionItemsInRoom(playerId);
    }

    private async Task<Guid> SetupForSpyglassProduction()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        await StartSpyglassProduction(playerId);
        return playerId;
    }

    private async Task StartSpyglassProduction(Guid playerId)
    {
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
            ActionTargets = []
        });
    }

    private async Task<ActionTarget> GetProductionTarget(Guid playerId)
    {
        ActionDto? cancelAction = await GetCancelProductionAction(playerId);
        return cancelAction.Prompts.First().ValidTargets.First();
    }

    private async Task<ActionDto?> GetCancelProductionAction(Guid playerId)
    {
        GetActionsResult actions = await Mediator.Send(new GetActionsRequest { PlayerId = playerId });
        return actions.Actions.FirstOrDefault(x => x.Name == ActionNames.CancelProduction);
    }

    private async Task CancelProduction(Guid playerId, ActionTarget target)
    {
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CancelProduction,
            PlayerId = playerId,
            ActionTargets = [[target]]
        });
    }

    private async Task AssertNoProductionItemsInRoom(Guid playerId)
    {
        RefreshScope();
        Player player = await GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        Assert.DoesNotContain(player.Room.Inventory.Items, x => x is ProductionItemBase);
    }

    // TODO: Put as base methodo of northwestIntegrationTestBase
    private void RefreshScope()
    {
        Scope = RootServiceProvider.CreateScope();
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
        Player player = await Scope.ServiceProvider.GetRequiredService<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);
        player.Inventory.Items.Add(new Scrap());
        Room room = await Context.Rooms.Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == roomenum);
        player.Room = room;
        await Context.SaveChangesAsync();
        return playerId;
    }
}