using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.Productions.HammerProduction.Contribution;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.HammerProduction.Contribution;

[TestSubject(typeof(HammerProductionContributionApp))]
public class HammerProductionContributionAppTest : NorthwestIntegrationTestBase
{
    private IPlayerRepository _playerRepository => this.GetServiceFromScope<IPlayerRepository>();

    public HammerProductionContributionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenWorkshopAndScrap_WhenContributingEnoughTimes_ThenHammerIsInRoomsInventory()
    {
        Guid playerId = await SetupUntilUnfinishedHammerCreated();


        /*
         * Try contributing until an exception is thrown.
         */
        try
        {
            Player p = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(playerId);
            while (!p.Room.Has<Hammer>())
            {
                await Mediator.Send(new ExecuteActionRequest
                {
                    ActionName = ActionNames.HammerProductionContribution,
                    PlayerId = playerId,
                    ActionTargets = [],
                });

                p = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(playerId);
            }
        }
        catch (Exception e)
        {
        }


        Player p2 = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(playerId);

        Assert.True(p2.Has<Hammer>());
    }


    private async Task<Guid> SetupUntilUnfinishedHammerCreated()
    {
        Guid playerId = await SetupForHammerContribution();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.HammerProductionInitiation,
            PlayerId = playerId,
        });

        return playerId;
    }

    /*
     * Giving 99999 contribution points.
     * Putting scrap in room to start the production.
     */
    private async Task<Guid> SetupForHammerContribution()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = await PlayerHelper.TeleportPlayerTo(this._scope, Context, gameDataSeed,
            HammerProductionInitiation.REQUIRED_ROOM);

        Player player = await base.GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        player.ActionPoints = 99999;
        player.Room.Inventory.Items.Add(new Scrap());

        await Context.SaveChangesAsync();

        return playerId;
    }
}