using AetherFire23.ERP.Domain.Features.Actions.Core;
using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.General.Combat;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Combat;

[TestSubject(typeof(CombatActionApp))]
public class CombatActionAppTest : NorthwestIntegrationTestBase
{
    public CombatActionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenPlayerInRoom_WhenGettingAvailability_ThenSomePlayersAreVisible()
    {
        // This test might break if I change spawn rooms locations. 
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);

        GetActionsResult act = await Mediator.Send(new GetActionsRequest()
        {
            PlayerId = gameDataSeed.PlayerIds[0]
        });


        int i = 0;
        // TODO; write assert

        bool hasTarget = act.Actions.First(x => x.Name == ActionNames.CombatAction)
            .Prompts.First()
            .ValidTargets.Any();

        Assert.True(hasTarget);
    }

    // TODO: Doesn't really have tests for real combat haha
}