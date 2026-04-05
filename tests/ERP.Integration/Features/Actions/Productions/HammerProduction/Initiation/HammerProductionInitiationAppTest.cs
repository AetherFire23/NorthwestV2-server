using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.HammerProduction.Initiation;

[TestSubject(typeof(HammerProductionInitiationApp))]
public class HammerProductionInitiationAppTest : NorthwestIntegrationTestBase
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
        Guid playerId = await PlayerHelper.TeleportPlayerTo(this._scope, Context, gameDataSeed,
            HammerProductionInitiation.REQUIRED_ROOM);

        Player player = await base.GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        player.Room.Inventory.Items.Add(new Scrap());

        await this.Context.SaveChangesAsync();
        return playerId;
    }
}