using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Productions.Core.Entities;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.ContributionToStages.Stages;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.SpyglassProduction;

[TestSubject(typeof(SpyglassProductionInitiationActionApp))]
public class SpyglassProductionInitiationAppTest : TestBase2
{
    public SpyglassProductionInitiationAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenPlayerWithScrapInInventory_WhenGetProductionAvailability_ThenCanExecuteAction()
    {
        Guid playerId = await SetupForSpyglassStartAction();

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        ActionDto spyglassStartAction = actions.Actions.First(x => x.Name == ActionNames.SpyglassProductionStart);
        bool allRequirementsFulfilled = spyglassStartAction.Requirements.All(x => x.IsFulfilled);
        Assert.True(allRequirementsFulfilled, "All requirements should be fulfilled to start spyglass production");
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

        this.Scope = this.RootServiceProvider.CreateScope();
        Player player = Context.Players.Include(x => x.Inventory).ThenInclude(x => x.Items)
            .First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .ThenInclude(x =>
                ((ProductionItemBase)x).LockedItems) // Trick to make sub-types collections load inside of ef. 
            .First(x => x.Id == player.RoomId);

        Assert.True(room.Inventory.Items.Any(x => x.ItemType == ItemTypes.UnfinishedSpyglass));
    }

    [Fact]
    public async Task GivenUnfinishedSpyglass_WhenAfterInitiated_ThenHasItsOwnFirstStage()
    {
        Guid playerId = await SetupForSpyglassStartAction();

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        this.Scope = this.RootServiceProvider.CreateScope();
        Player player = Context.Players.First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == player.RoomId);
        UnfinishedSpyglass unfinishedSpyglass = room.Inventory.Find(ItemTypes.UnfinishedSpyglass) as UnfinishedSpyglass;
        Assert.True(unfinishedSpyglass.CurrentStageContribution is SpyglassFirstStageContributionData);
    }

    [Fact]
    public async Task GivenUnfinishedSpyglass_WhenAfterInitiated_ThenHasScrapLockedItem()
    {
        Guid playerId = await SetupForSpyglassStartAction();

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        this.Scope = this.RootServiceProvider.CreateScope();
        Player player = Context.Players.First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .ThenInclude(x => ((ProductionItemBase)x).LockedItems)
            .First(x => x.Id == player.RoomId);
        UnfinishedSpyglass unfinishedSpyglass = (UnfinishedSpyglass)room.Inventory.Find(ItemTypes.UnfinishedSpyglass);
        Assert.True(unfinishedSpyglass.LockedItems.Any(x => x.ItemType == ItemTypes.Scrap));
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
        var room = await Context.Rooms.Where(x => x.GameId == player.GameId)
            .FirstAsync(x => x.RoomEnum == roomenum);
        player.Room = room;
        await Context.SaveChangesAsync();
        return playerId;
    }
}