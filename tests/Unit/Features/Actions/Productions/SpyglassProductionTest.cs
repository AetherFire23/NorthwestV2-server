using ERP.Testing.Domain.Dummies;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution.Stages;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;

namespace ERP.Testing.Domain.Features.Actions.Productions;

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
            ItemType = ItemTypes.UnfinishedSpyglass, CurrentStageContribution = new SpyglassFirstStageContributionData()
        };
    }

    private static UnfinishedSpyglass CreateSpyglassAtSecondStage()
    {
        return new UnfinishedSpyglass
        {
            ItemType = ItemTypes.UnfinishedSpyglass, CurrentStageContribution = new SpyglassSecondStageContributionData()
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

    [Fact]
    public void GivenSecondStage_WhenGettingSentryAvailability_ThenIsExecutable()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.ForeCastle);
        sentry.Room.Inventory.Add(CreateSpyglassAtSecondStage());
        SpyglassProductionContributionAction action = new();

        InstantActionAvailability? availability = action.DetermineSentryAvailability(sentry);

        Assert.NotNull(availability);
        Assert.Equal(ActionNames.SpyglassSentryContribution, availability.ActionName);
        Assert.True(availability.CanExecute);
    }

    [Fact]
    public void GivenFirstStage_WhenGettingSentryAvailability_ThenIsNotOffered()
    {
        Player sentry = CreatePlayer(Roles.Sentry, RoomEnum.Workshop);
        sentry.Room.Inventory.Add(CreateSpyglassAtFirstStage());
        SpyglassProductionContributionAction action = new();

        InstantActionAvailability? availability = action.DetermineSentryAvailability(sentry);

        Assert.Null(availability);
    }

    [Fact]
    public void GivenSecondStageWithFullScholarQuota_WhenGettingScholarAvailability_ThenCannotExecute()
    {
        Player scholar = CreatePlayer(Roles.Scholar, RoomEnum.ForeCastle);
        UnfinishedSpyglass spyglass = CreateSpyglassAtSecondStage();
        scholar.Room.Inventory.Add(spyglass);
        for (int i = 0; i < SpyglassSecondStageContributionData.SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT; i++)
        {
            spyglass.Contribute(scholar, Roles.Scholar);
        }

        SpyglassProductionContributionAction action = new();

        InstantActionAvailability? availability = action.DetermineAvailability(scholar);

        Assert.NotNull(availability);
        Assert.False(availability.CanExecute);
    }
}
