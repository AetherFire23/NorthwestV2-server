using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.SpyglassProduction;

[TestSubject(typeof(SpyglassProductionInitiationActionApp))]
public class SpyglassProductionInitiationAppTest : NorthwestIntegrationTestBase
{
    public SpyglassProductionInitiationAppTest(ITestOutputHelper output) : base(output)
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
    public async Task GivenPlayerWithScrapInInventory_WhenGetProductionAvailability_ThenCanExecuteAction()
    {
        Guid playerId = await SetupForSpyglassStartAction();

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        Assert.True(actions.Actions.First(x => x.Name == ActionNames.SpyglassProductionStart).Requirements
            .All(x => x.IsFulfilled));
    }

    [Fact]
    public async Task
        GivenPlayerWithScrapInInventory_WhenActionExecuted_ThenIsUnfinishedSpyglassCreatedInRoomsInventory()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        var actions = await Mediator.Send(new ExecuteActionRequest()
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        this._scope = this.RootServiceProvider.CreateScope();
        Player player = Context.Players.First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == player.RoomId);

        Assert.True(room.Inventory.Items.Any(x => x.ItemType == ItemTypes.UnfinishedSpyglass));
    }

    // TODO: Test for checking that a first stage is created inside the unfinished spyglass.
    [Fact]
    public async Task GivenUnfinishedSpyglass_WhenAfterInitiated_ThenHasItsOwnFirstStage()
    {
        Guid playerId = await SetupForSpyglassStartAction();

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        this._scope = this.RootServiceProvider.CreateScope();
        Player player = Context.Players.First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == player.RoomId);
        // TODO: make sure that ef core understands Stages. I guess it can be a DbSet<> But Technicallyt it is just a tracker for an integer
        // Also, it would be interesting to know if sql can actually handle all the plymoprphism there 
        UnfinishedSpyglass unfinishedSpyglass = room.Inventory.Find(ItemTypes.UnfinishedSpyglass) as UnfinishedSpyglass;
        Assert.True(unfinishedSpyglass.CurrentStage is SpyglassFirstStageData);
    }

    [Fact]
    public async Task GivenFirstStage_WhenContributingOnce_ThenHasPointsContributed()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassContribution,
            PlayerId = playerId,
        });

        this._scope = this.RootServiceProvider.CreateScope();
        Player player = Context.Players.First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == player.RoomId);
        
        UnfinishedSpyglass unfinishedSpyglass = room.Inventory.Find(ItemTypes.UnfinishedSpyglass) as UnfinishedSpyglass;
        
        Assert.Equal(1, unfinishedSpyglass.CurrentStage.Contributions);
    }
    // TODO: Combine the new SpyglassFirstSTageData


    // TODO: TEst that we can contribute 1 point

    // TODO: TEst that when we reach the next stage, we cannot contribute more points

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