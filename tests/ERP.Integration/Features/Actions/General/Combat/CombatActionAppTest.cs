using JetBrains.Annotations;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Combat;

[TestSubject(typeof(CombatActionApp))]
public class CombatActionAppTest : TestBase2
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

        bool hasTarget = act.Actions.First(x => x.Name == ActionNames.CombatAction)
            .Prompts.First()
            .ValidTargets.Any();

        Assert.True(hasTarget);
    }

    /*
     * Tests for choosing the target selection of the defensive counter
     */

    [Fact]
    public async Task GivenPlayerInRoom_WhenGettingCombatAction_ThenCanChooseDefensiveStance()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid playerId = gameDataSeed.PlayerIds.First();
        GetActionsResult combtatAction = await Mediator.Send(new GetActionsRequest()
        {
            PlayerId = playerId
        });

        bool isAttackerStanceCountAvailableAsTargets =
            combtatAction.Actions.First(x => x.Name == ActionNames.CombatAction).Prompts[1].ValidTargets.Count ==
            Enum.GetValues<AttackerStances>().Length;

        Assert.True(isAttackerStanceCountAvailableAsTargets);
    }

    /*
     * Tests for attacking
     * TODO: consider making more setup logic to put the defender and attacker into the same room.
     */
    [Fact]
    public async Task GivenPlayerInRoom_WhenAttackingOtherPlayerInRoom_ThenFightEndsWithoutAnyErrors()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid playerId = gameDataSeed.PlayerIds.First();
        GetActionsResult combtatAction = await Mediator.Send(new GetActionsRequest()
        {
            PlayerId = playerId
        });
        ActionDto actionDto = combtatAction.Actions.First(x => x.Name == ActionNames.CombatAction);
        ActionTarget player = actionDto.Prompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets.First();
        ActionTarget attackerStance = actionDto.Prompts[CombatAction.ATTACKERSTANCE_PROMPT_INDEX].ValidTargets.First();

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CombatAction,
            PlayerId = playerId,
            ActionTargets = [[player], [attackerStance]]
        });

        Assert.True(true);
    }

    // TODO:
    /*
     * What happens when ytou die ?
     * Dead body retrievable, just like a IsDead property ?
     * It's not defined.
     */
}