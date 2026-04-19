using JetBrains.Annotations;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.FishingPole.Initiation;

[TestSubject(typeof(FishingPoleInitiationApp))]
public class FishingPoleInitiationAppTest : TestBase2
{
    public FishingPoleInitiationAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenScrapAndPlayerInWorkshop_WhenGettingActions_ThenUnfinishedFishingRodIsInRoomInventory()
    {
        Guid playerId = await SetupForFishingRodInitiation();

        GetActionsResult gettingActionsReuslt = await Mediator.Send(new GetActionsRequest()
        {
            PlayerId = playerId,
        });

        Assert.True(gettingActionsReuslt.HasAction(ActionNames.InitiateFishingPoleProduction));
    }


    [Fact]
    public async Task GivenScrap_WhenInitiating_ThenUnfinishedFishingRodIsInRoomInventory()
    {
        Guid playerId = await SetupForFishingRodInitiation();

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.InitiateFishingPoleProduction,
            PlayerId = playerId,
        });


        Player player = this.Context.Players.First(x => x.Id == playerId);

        Assert.True(player.Room.HasItemOfType<UnfinishedFishingPole>());
    }

    private async Task<Guid> SetupForFishingRodInitiation()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = await PlayerHelper.TeleportPlayerTo(
            Scope,
            Context,
            gameDataSeed,
            FishingPoleStage.REQUIRED_ROOM);

        Player player = await base.GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        player.Room.Inventory.Items.Add(new Scrap());

        await Context.SaveChangesAsync();
        return playerId;
    }
}