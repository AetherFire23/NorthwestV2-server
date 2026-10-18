using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.Productions.SpyglassProduction.Contribution;
using NorthwestV2.Features.Actions.Productions.SpyglassProduction.Contribution.Stages;
using NorthwestV2.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.Productions;

public class SpyglassProductionTest
{
    private static Player CreatePlayer(Roles role, RoomEnum room, int actionPoints = 99)
    {
        Game game = TestPlayers.CreateTestGame();
        Room playersRoom = TestPlayers.CreateTestRoom(room, game);
        Player player = TestPlayers.CreateTestPlayer(10, role, playersRoom, game);
        player.ActionPoints = actionPoints;
        return player;
    }

    private static UnfinishedSpyglass CreateSpyglassAtFirstStage()
    {
        return new UnfinishedSpyglass
        {
            Id = Guid.NewGuid(), ItemType = ItemTypes.UnfinishedSpyglass,
            CurrentStageContribution = new SpyglassFirstStageContributionData()
        };
    }

    private static UnfinishedSpyglass CreateSpyglassAtSecondStage()
    {
        return new UnfinishedSpyglass
        {
            Id = Guid.NewGuid(), ItemType = ItemTypes.UnfinishedSpyglass,
            CurrentStageContribution = new SpyglassSecondStageContributionData()
        };
    }

    [Fact]
    public void GivenNonSpecialistWithTooFewPoints_WhenContributing_ThenThrowsAndPointsAreKept()
    {
        Player scholar = CreatePlayer(Roles.Scholar, RoomEnum.Workshop, actionPoints: 2);
        UnfinishedSpyglass spyglass = CreateSpyglassAtFirstStage();

        Assert.ThrowsAny<Exception>(() => spyglass.Contribute(scholar));

        Assert.Equal(2, scholar.ActionPoints);
        Assert.Equal(0, spyglass.CurrentStageContribution.RequiredContributions);
    }

    [Fact]
    public void GivenQuartermasterWithOnePoint_WhenContributing_ThenThrows()
    {
        Player quartermaster = CreatePlayer(Roles.QuarterMaster, RoomEnum.Workshop, actionPoints: 1);
        UnfinishedSpyglass spyglass = CreateSpyglassAtFirstStage();

        Assert.ThrowsAny<Exception>(() => spyglass.Contribute(quartermaster));
    }

    [Theory]
    [InlineData(Roles.Scholar, Roles.Scholar, 1)]
    [InlineData(Roles.QuarterMaster, Roles.Scholar, 2)]
    [InlineData(Roles.Sentry, Roles.Scholar, 3)]
    [InlineData(Roles.Sentry, Roles.Sentry, 1)]
    [InlineData(Roles.QuarterMaster, Roles.Sentry, 2)]
    [InlineData(Roles.Scholar, Roles.Sentry, 3)]
    public void GivenSecondStage_WhenContributingToAQuota_ThenCostDependsOnPlayersRoleVersusQuotaRole(
        Roles playerRole, Roles quotaRole, int expectedCost)
    {
        Player player = CreatePlayer(playerRole, RoomEnum.ForeCastle, actionPoints: 10);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();

        spyglass.Contribute(player, quotaRole);

        Assert.Equal(10 - expectedCost, player.ActionPoints);
    }

    [Fact]
    public void GivenSecondStage_WhenOnlyScholarQuotaIsFull_ThenStageDoesNotAdvance()
    {
        Player scholar = CreatePlayer(Roles.Scholar, RoomEnum.ForeCastle);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();

        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            spyglass.Contribute(scholar, Roles.Scholar);
        }

        Assert.IsType<SpyglassSecondStageContributionData>(spyglass.CurrentStageContribution);
    }

    [Fact]
    public void GivenSecondStage_WhenOnlySentryQuotaIsFull_ThenStageDoesNotAdvance()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.ForeCastle);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();

        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_SENTRY_CONTRIBUTION_LIMIT; i++)
        {
            spyglass.Contribute(sentry, Roles.Sentry);
        }

        Assert.IsType<SpyglassSecondStageContributionData>(spyglass.CurrentStageContribution);
    }

    [Fact]
    public void GivenSecondStage_WhenBothQuotasAreFull_ThenAdvancesToThirdStage()
    {
        Player scholar = CreatePlayer(Roles.Scholar, RoomEnum.ForeCastle);
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.ForeCastle);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();

        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_SENTRY_CONTRIBUTION_LIMIT; i++)
        {
            spyglass.Contribute(sentry, Roles.Sentry);
        }

        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            spyglass.Contribute(scholar, Roles.Scholar);
        }

        Assert.IsType<SpyglassProductionThirdStageContributionData>(spyglass.CurrentStageContribution);
    }

    [Fact]
    public void GivenFullSentryQuota_WhenContributingToItAgain_ThenThrowsAndPointsAreKept()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.ForeCastle);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();
        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_SENTRY_CONTRIBUTION_LIMIT; i++)
        {
            spyglass.Contribute(sentry, Roles.Sentry);
        }

        int pointsBefore = sentry.ActionPoints;

        Assert.ThrowsAny<Exception>(() => spyglass.Contribute(sentry, Roles.Sentry));
        Assert.Equal(pointsBefore, sentry.ActionPoints);
    }

    [Fact]
    public void GivenFirstStage_WhenContributingToSentryQuota_ThenThrows()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.Workshop);
        UnfinishedSpyglass spyglass = CreateSpyglassAtFirstStage();

        Assert.ThrowsAny<Exception>(() => spyglass.Contribute(sentry, Roles.Sentry));
    }

    private static ActionTargetsList TargetsFor(UnfinishedSpyglass spyglass, Roles quotaRole)
    {
        return ActionTargetsList.From(
        [
            [spyglass.ToTarget()],
            [new ActionTarget { Name = quotaRole.ToString(), Value = quotaRole.ToString(), TargetId = Guid.Empty }]
        ]);
    }

    private static List<Roles> QuotaPromptRoles(ActionWithTargetsAvailability availability)
    {
        return availability.TargetSelectionPrompts[SpyglassProductionContributionAction.QUOTA_PROMPT_INDEX]
            .ValidTargets.Select(x => Enum.Parse<Roles>(x.Value!)).ToList();
    }

    [Fact]
    public void GivenNoUnfinishedSpyglassInRoom_WhenGettingAvailability_ThenIsNotOffered()
    {
        Player engineer = CreatePlayer(Roles.Engineer, RoomEnum.Workshop);

        ActionWithTargetsAvailability? availability = new SpyglassProductionContributionAction().DetermineAvailability(engineer);

        Assert.Null(availability);
    }

    [Fact]
    public void GivenFirstStage_WhenGettingAvailability_ThenQuotaPromptOffersOnlyEngineer()
    {
        Player engineer = CreatePlayer(Roles.Engineer, RoomEnum.Workshop);
        engineer.Room.Inventory.Add(CreateSpyglassAtFirstStage());

        ActionWithTargetsAvailability availability =
            new SpyglassProductionContributionAction().DetermineAvailability(engineer)!;

        Assert.Equal([Roles.Engineer], QuotaPromptRoles(availability));
        Assert.True(availability.CanExecute);
    }

    [Fact]
    public void GivenSecondStage_WhenGettingAvailability_ThenQuotaPromptOffersScholarAndSentry()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.ForeCastle);
        sentry.Room.Inventory.Add(CreateSpyglassAtSecondStage());

        ActionWithTargetsAvailability availability =
            new SpyglassProductionContributionAction().DetermineAvailability(sentry)!;

        Assert.Equal([Roles.Scholar, Roles.Sentry], QuotaPromptRoles(availability));
        Assert.True(availability.CanExecute);
        Assert.Equal(SpyglassSecondStageContributionData.DESCRIPTION, availability.DisplayName);
    }

    [Fact]
    public void GivenSecondStageWithFullScholarQuota_WhenGettingAvailability_ThenQuotaPromptOffersOnlySentry()
    {
        Player scholar = CreatePlayer(Roles.Scholar, RoomEnum.ForeCastle);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();
        scholar.Room.Inventory.Add(spyglass);
        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            spyglass.Contribute(scholar, Roles.Scholar);
        }

        ActionWithTargetsAvailability availability =
            new SpyglassProductionContributionAction().DetermineAvailability(scholar)!;

        Assert.Equal([Roles.Sentry], QuotaPromptRoles(availability));
    }

    [Fact]
    public void GivenTwoUnfinishedSpyglasses_WhenGettingAvailability_ThenSpyglassPromptListsBoth()
    {
        Player engineer = CreatePlayer(Roles.Engineer, RoomEnum.Workshop);
        engineer.Room.Inventory.Add(CreateSpyglassAtFirstStage());
        engineer.Room.Inventory.Add(CreateSpyglassAtFirstStage());

        ActionWithTargetsAvailability availability =
            new SpyglassProductionContributionAction().DetermineAvailability(engineer)!;

        Assert.Equal(2,
            availability.TargetSelectionPrompts[SpyglassProductionContributionAction.SPYGLASS_PROMPT_INDEX]
                .ValidTargets.Count);
    }

    [Fact]
    public void GivenPlayerWithoutEnoughPointsForTheCheapestQuota_WhenGettingAvailability_ThenCannotExecute()
    {
        Player cook = CreatePlayer(Roles.Cook, RoomEnum.Workshop, actionPoints: 2);
        cook.Room.Inventory.Add(CreateSpyglassAtFirstStage());

        ActionWithTargetsAvailability availability =
            new SpyglassProductionContributionAction().DetermineAvailability(cook)!;

        Assert.False(availability.CanExecute);
    }

    [Fact]
    public void GivenPlayerInTheWrongRoom_WhenGettingAvailability_ThenCannotExecute()
    {
        Player scholar = CreatePlayer(Roles.Scholar, RoomEnum.Workshop);
        scholar.Room.Inventory.Add(CreateSpyglassAtSecondStage());

        ActionWithTargetsAvailability availability =
            new SpyglassProductionContributionAction().DetermineAvailability(scholar)!;

        Assert.False(availability.CanExecute);
    }

    [Fact]
    public void GivenSelectedSpyglassAndSentryQuota_WhenContributing_ThenOnlySentryQuotaAdvances()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.ForeCastle);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();
        sentry.Room.Inventory.Add(spyglass);

        new SpyglassProductionContributionAction().Contribute(sentry, TargetsFor(spyglass, Roles.Sentry));

        var stage = (SpyglassSecondStageContributionData)spyglass.CurrentStageContribution;
        Assert.Equal(1, stage.SentryContributions);
        Assert.Equal(0, stage.RequiredContributions);
    }

    [Fact]
    public void GivenSelectedQuotaNotOfferedByStage_WhenContributing_ThenThrows()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.Workshop);
        UnfinishedSpyglass spyglass = CreateSpyglassAtFirstStage();
        sentry.Room.Inventory.Add(spyglass);

        Assert.ThrowsAny<Exception>(() =>
            new SpyglassProductionContributionAction().Contribute(sentry, TargetsFor(spyglass, Roles.Sentry)));
    }
}
