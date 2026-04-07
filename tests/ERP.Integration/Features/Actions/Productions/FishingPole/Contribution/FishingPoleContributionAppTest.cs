using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.FishingPole.Contribution;

[TestSubject(typeof(FishingPoleContributionApp))]
public class FishingPoleContributionAppTest : TestBase2
{
    public FishingPoleContributionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenUnfinishedSpyglass_WhenGettingActions_ThenContributionActionAvailable()
    {
        Guid playerId = await SetupForFishingRodContribution();

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });


        Assert.True(actions.HasAction(ActionNames.ContributeFishingPoleProduction));
    }

    /// <summary>
    /// Give unlimited timepoints to player
    /// teleport torequired room
    /// Produces the unfinished item. 
    /// </summary>
    /// <returns></returns>
    private async Task<Guid> SetupForFishingRodContribution()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = await PlayerHelper.TeleportPlayerTo(
            Scope,
            Context,
            gameDataSeed,
            FishingPoleStage.REQUIRED_ROOM);

        Player player = await base.GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        player.ActionPoints = 999999;

        player.Room.Inventory.Items.Add(new Scrap());

        await Mediator.Send(new ExecuteActionRequest()
        {
            ActionName = ActionNames.InitiateFishingPoleProduction,
            PlayerId = playerId,
        });

        await Context.SaveChangesAsync();
        return playerId;
    }
}