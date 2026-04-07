using JetBrains.Annotations;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.HammerProduction.Contribution;

[TestSubject(typeof(HammerProductionContributionApp))]
public class HammerProductionContributionAppTest : TestBase2
{
    private IPlayerRepository _playerRepository => this.GetServiceFromScope<IPlayerRepository>();

    public HammerProductionContributionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenWorkshopAndScrap_WhenContributingUntilCompleted_ThenHammerIsInPlayerInventory()
    {
        Guid playerId = await SetupUntilUnfinishedHammerCreated();

        // await ContributeUntilHammerCompletedAsync(playerId);

        await ContributeUntilItemInInventoryAsync(
            playerId,
            ActionNames.HammerProductionContribution,
            x => x.Room.Has<Hammer>()
        );

        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(playerId);
        Assert.True(player.Has<Hammer>());
    }

    private async Task ContributeUntilItemInInventoryAsync(
        Guid playerId,
        string actionName,
        Func<Player, bool> itemCheck)
    {
        try
        {
            Player p = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(playerId);
            while (!itemCheck(p))
            {
                await Mediator.Send(new ExecuteActionRequest
                {
                    ActionName = actionName,
                    PlayerId = playerId,
                    ActionTargets = [],
                });
                p = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(playerId);
            }
        }
        catch (Exception e)
        {
        }
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
        Guid playerId = await PlayerHelper.TeleportPlayerTo(this.Scope, Context, gameDataSeed,
            HammerProductionInitiation.REQUIRED_ROOM);

        Player player = await base.GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        player.ActionPoints = 99999;
        player.Room.Inventory.Items.Add(new Scrap());

        await Context.SaveChangesAsync();

        return playerId;
    }

    private async Task ContributeUntilHammerCompletedAsync(Guid playerId)
    {
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
    }
}