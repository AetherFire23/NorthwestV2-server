using JetBrains.Annotations;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.HammerProduction.Initiation;

[TestSubject(typeof(HammerProductionInitiationApp))]
public class HammerProductionInitiationAppTest : TestBase2
{
    /*
     * Hammer
     * Single Stage – Workshop: 1 scrap, low Engineer TP.
     * Effect: Produces a hammer.
     */
    public HammerProductionInitiationAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenWorkshopAndScrapInRoom_WhenGettingActions_ThenIsHammerProductionInitiationPresent()
    {
        Guid playerId = await SetupForHammerProduction();

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        Assert.True(actions.GetAction(ActionNames.HammerProductionInitiation).IsExecutable());
    }

    [Fact]
    public async Task GivenWorkshopAndScrapInRoom_WhenInitiatingProduction_ThenIsUnfinishedHammerPresent()
    {
        Guid playerId = await SetupForHammerProduction();

        var actions = await Mediator.Send(new ExecuteActionRequest()
        {
            PlayerId = playerId,
            ActionName = ActionNames.HammerProductionInitiation,
            ActionTargets = [],
        });

        Player player = await GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);
        Assert.True(player.Room.Has(ItemTypes.UnfinishedHammer));
    }

    private async Task<Guid> SetupForHammerProduction()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = await PlayerHelper.TeleportPlayerTo(this.Scope, Context, gameDataSeed,
            HammerProductionInitiation.REQUIRED_ROOM);

        Player player = await base.GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        player.Room.Inventory.Items.Add(new Scrap());

        await this.Context.SaveChangesAsync();
        return playerId;
    }
}