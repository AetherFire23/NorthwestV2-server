using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution.Stages;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.SpyglassProduction;

[TestSubject(typeof(SpyglassProductionInitiationActionApp))]
public class SpyglassProductionContributionAppTest : TestBase2
{
    public SpyglassProductionContributionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenSpyglassProductionInitiated_WhenGetActions_ThenContributionActionIsAvailable()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        ActionDto contributionAction = actions.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
        bool allRequirementsFulfilled = contributionAction.Requirements.All(x => x.IsFulfilled);
        Assert.True(allRequirementsFulfilled, "Contribution action should be available after initiation");
    }

    [Fact]
    public async Task GivenSpyglassProductionInitiated_WhenGetActions_ThenDescriptionContainsFirstStageName()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        ActionDto contributionAction = actions.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
        Assert.Equal(SpyglassFirstStageContributionData.DESCRIPTION, contributionAction.Description);
    }

    [Fact]
    public async Task GivenFirstStage_WhenContributingOnce_ThenHasPointsContributed()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player = await Context.Players.FirstAsync(x => x.Id == playerId);
        player.ActionPoints = 99;
        await Context.SaveChangesAsync();
        Player pp = await this.Scope.ServiceProvider.GetRequiredService<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);
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

        this.Scope = this.RootServiceProvider.CreateScope();
        Player playerAfter = Context.Players.First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == playerAfter.RoomId);
        UnfinishedSpyglass unfinishedSpyglass = room.Inventory.Find(ItemTypes.UnfinishedSpyglass) as UnfinishedSpyglass;
        Assert.Equal(1, unfinishedSpyglass.CurrentStageContribution.RequiredContributions);
    }


    /*
     *
     */
    [Fact]
    public async Task GivenFirstStage_WhenContributingFullFirstStageLimit_ThenSecondStageDescriptionIsDisplayed()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player = await Context.Players.FirstAsync(x => x.Id == playerId);
        player.ActionPoints = 99;
        await Context.SaveChangesAsync();

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });
        for (int i = 0; i < SpyglassFirstStageContributionData.SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
            {
                ActionName = ActionNames.SpyglassContribution,
                PlayerId = playerId,
            });
        }

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        ActionDto contributionAction = actions.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
        var expectedSecondStageName = new SpyglassSecondStageContributionData().StageName;
        Assert.Equal(expectedSecondStageName, contributionAction.Description);
    }

    [Fact]
    public async Task GivenFirstStage_WhenContributingEnoughToReachThirdStage_ThenAdvancesToThirdStage()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player = await Context.Players.FirstAsync(x => x.Id == playerId);
        player.ActionPoints = 99;
        await Context.SaveChangesAsync();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        for (int i = 0; i < SpyglassFirstStageContributionData.SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
            {
                ActionName = ActionNames.SpyglassContribution,
                PlayerId = playerId,
            });
        }

        await TeleportPlayerToRoom(playerId, SpyglassSecondStageContributionData.REQUIRED_ROOM);

        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
            {
                ActionName = ActionNames.SpyglassContribution,
                PlayerId = playerId,
            });
        }

        await TeleportPlayerToRoom(playerId, SpyglassProductionThirdStageContributionData.REQUIRED_ROOM);

        this.Scope = this.RootServiceProvider.CreateScope();
        Player playerAfter = Context.Players.First(x => x.Id == playerId);
        Room room = Context.Rooms
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == playerAfter.RoomId);
        UnfinishedSpyglass unfinishedSpyglass = room.Inventory.Find(ItemTypes.UnfinishedSpyglass) as UnfinishedSpyglass;
        Assert.True(unfinishedSpyglass.CurrentStageContribution is SpyglassProductionThirdStageContributionData);
    }

    [Fact]
    public async Task GivenFirstStage_WhenContributingFullFirstTwoStages_ThenThirdStageDescriptionIsDisplayed()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player = await Context.Players.FirstAsync(x => x.Id == playerId);
        player.ActionPoints = 99;
        await Context.SaveChangesAsync();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        for (int i = 0; i < SpyglassFirstStageContributionData.SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
            {
                ActionName = ActionNames.SpyglassContribution,
                PlayerId = playerId,
            });
        }

        await TeleportPlayerToRoom(playerId, SpyglassSecondStageContributionData.REQUIRED_ROOM);

        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
            {
                ActionName = ActionNames.SpyglassContribution,
                PlayerId = playerId,
            });
        }

        await TeleportPlayerToRoom(playerId, SpyglassProductionThirdStageContributionData.REQUIRED_ROOM);

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        ActionDto contributionAction = actions.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
        var expectedThirdStageName = SpyglassProductionThirdStageContributionData.DESCRIPTION;
        Assert.Equal(expectedThirdStageName, contributionAction.Description);
    }

    private const int SPYGLASS_FIRST_TWO_STAGES_CONTRIBUTION_LIMIT =
        SpyglassFirstStageContributionData.SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT +
        SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT;

    private const int SPYGLASS_ALL_STAGES_CONTRIBUTION_LIMIT =
        SPYGLASS_FIRST_TWO_STAGES_CONTRIBUTION_LIMIT +
        SpyglassProductionThirdStageContributionData.SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT;

    [Fact]
    public async Task
        GivenFirstStage_WhenContributingEnoughToReachThirdStage_ThenFinishedSpyglassInsideExistsPlayersInventory()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player = await Context.Players.FirstAsync(x => x.Id == playerId);
        player.ActionPoints = 999;
        await Context.SaveChangesAsync();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        for (int i = 0; i < SpyglassFirstStageContributionData.SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        await TeleportPlayerToRoom(playerId, SpyglassSecondStageContributionData.REQUIRED_ROOM);
        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        await TeleportPlayerToRoom(playerId, SpyglassProductionThirdStageContributionData.REQUIRED_ROOM);
        for (int i = 0; i < SpyglassProductionThirdStageContributionData.SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        this.Scope = this.RootServiceProvider.CreateScope();
        Player playerAfter = Context.Players
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == playerId);
        Assert.True(playerAfter.Inventory.Items.Any(x => x.ItemType == ItemTypes.Spyglass));
    }

    [Fact]
    public async Task
        GivenPlayerInCorrectRoom_WhenCompletingFullSpyglassProductionInOneSequence_ThenSpyglassInPlayersInventory()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player = await Context.Players.FirstAsync(x => x.Id == playerId);
        player.ActionPoints = 999;
        await Context.SaveChangesAsync();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        await ContributePointsUntilCompletion(playerId);

        this.Scope = this.RootServiceProvider.CreateScope();
        Player playerAfter = Context.Players
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == playerId);
        Assert.Contains(playerAfter.Inventory.Items, x => x.ItemType == ItemTypes.Spyglass);
    }

    [Fact]
    public async Task GivenMaxContributionsReached_WhenContributingMOre_ThenStagesChanged()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        Assert.True(actions.Actions.Any());
    }

    [Fact]
    public async Task GivenLastContribution_WhenCompleted_ThenIsUnfinishedItemAbsentFromRoomInventory()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player =
            await this.GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);
        player.ActionPoints = 999999;
        await this.Context.SaveChangesAsync();
        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        await ContributeUntillError(playerId);
        await TeleportPlayerToRoom(playerId, SpyglassSecondStageContributionData.REQUIRED_ROOM);

        await ContributeUntillError(playerId);
        await TeleportPlayerToRoom(playerId, SpyglassProductionThirdStageContributionData.REQUIRED_ROOM);

        await ContributeUntillError(playerId);
        Player playerAfter =
            await this.GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);

        Assert.False(playerAfter.Room.Has<UnfinishedSpyglass>());
    }

    private async Task ContributeUntillError(Guid playerId)
    {
        Player pBefore =
            await this.GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);
        try
        {
            while (!pBefore.Inventory.Has<Spyglass>())
            {
                await Mediator.Send(new ExecuteActionRequest
                    { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });

                pBefore =
                    await this.GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);
            }
        }
        catch (Exception e)
        {
        }
    }

    [Fact]
    public async Task GivenFullProductionFlow_WhenContributingStepByStep_ThenAllStagesTransitionCorrectly()
    {
        Guid playerId = await SetupForSpyglassStartAction();
        Player player = await Context.Players.FirstAsync(x => x.Id == playerId);
        player.ActionPoints = 999999;
        await Context.SaveChangesAsync();

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SpyglassProductionStart,
            PlayerId = playerId,
        });

        GetActionsResult actionsAfterInitiation = await Mediator.Send(new GetActionsRequest { PlayerId = playerId });
        ActionDto contributionActionAfterInitiation =
            actionsAfterInitiation.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
        Assert.Equal(SpyglassFirstStageContributionData.DESCRIPTION, contributionActionAfterInitiation.Description);

        int totalContributions = SPYGLASS_ALL_STAGES_CONTRIBUTION_LIMIT;

        for (int i = 0; i < SpyglassFirstStageContributionData.SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            GetActionsResult actionsBefore = await Mediator.Send(new GetActionsRequest { PlayerId = playerId });
            ActionDto contributionAction = actionsBefore.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
            Assert.True(contributionAction.Requirements.All(r => r.IsFulfilled), $"First stage, contribution {i + 1}");
            Assert.Equal(SpyglassFirstStageContributionData.DESCRIPTION, contributionAction.Description);

            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        await TeleportPlayerToRoom(playerId, SpyglassSecondStageContributionData.REQUIRED_ROOM);

        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            GetActionsResult actionsBefore = await Mediator.Send(new GetActionsRequest { PlayerId = playerId });
            ActionDto contributionAction = actionsBefore.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
            Assert.True(contributionAction.Requirements.All(r => r.IsFulfilled), $"Second stage, contribution {i + 1}");
            Assert.Equal(SpyglassSecondStageContributionData.DESCRIPTION, contributionAction.Description);

            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        await TeleportPlayerToRoom(playerId, SpyglassProductionThirdStageContributionData.REQUIRED_ROOM);

        for (int i = 0; i < SpyglassProductionThirdStageContributionData.SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            GetActionsResult actionsBefore = await Mediator.Send(new GetActionsRequest { PlayerId = playerId });
            ActionDto contributionAction = actionsBefore.Actions.First(x => x.Name == ActionNames.SpyglassContribution);
            Assert.True(contributionAction.Requirements.All(r => r.IsFulfilled), $"Third stage, contribution {i + 1}");
            Assert.Equal(SpyglassProductionThirdStageContributionData.DESCRIPTION, contributionAction.Description);

            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        this.Scope = this.RootServiceProvider.CreateScope();
        Player playerAfter = Context.Players
            .Include(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .First(x => x.Id == playerId);

        Assert.Contains(playerAfter.Inventory.Items, x => x.ItemType == ItemTypes.Spyglass);
    }

    private async Task ContributePointsUntilCompletion(Guid playerId)
    {
        for (int i = 0; i < SpyglassFirstStageContributionData.SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        await TeleportPlayerToRoom(playerId, SpyglassSecondStageContributionData.REQUIRED_ROOM);
        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }

        await TeleportPlayerToRoom(playerId, SpyglassProductionThirdStageContributionData.REQUIRED_ROOM);
        for (int i = 0; i < SpyglassProductionThirdStageContributionData.SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            await Mediator.Send(new ExecuteActionRequest
                { ActionName = ActionNames.SpyglassContribution, PlayerId = playerId });
        }
    }


    private async Task<Guid> SetupForSpyglassStartAction()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = await TeleportPlayerTo(gameDataSeed,
            SpyglassFirstStageContributionData.REQUIRED_ROOM);
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

    private async Task TeleportPlayerToRoom(Guid playerId, RoomEnum roomenum)
    {
        Player player = await Context.Players
            .Include(x => x.Game)
            .Include(player => player.Room).ThenInclude(room => room.Inventory)
            .FirstAsync(x => x.Id == playerId);
        Room room = await Context.Rooms.Where(x => x.GameId == player.GameId).Include(room => room.Inventory)
            .FirstAsync(x => x.RoomEnum == roomenum);

        UnfinishedSpyglass unfinishedSpyglass = player.Room.Inventory.Find<UnfinishedSpyglass>();

        player.Room.Inventory.Items.Remove(unfinishedSpyglass);
        room.Inventory.Items.Add(unfinishedSpyglass);

        player.Room = room;
        await Context.SaveChangesAsync();
    }
}