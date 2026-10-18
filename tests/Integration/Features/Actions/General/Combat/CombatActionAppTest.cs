using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.StartCombat;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Combat;

[TestSubject(typeof(CombatActionApp))]
public class CombatActionAppTest : NorthwestTestBase
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
    public async Task GivenPlayerInRoom_WhenGettingCombatAction_ThenCanChooseAnAttackType()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid playerId = gameDataSeed.PlayerIds.First();

        ActionDto actionDto = await GetCombatAction(playerId);

        Assert.Equal(Enum.GetValues<AttackTypes>().Length,
            actionDto.Prompts[CombatAction.ATTACKTYPE_PROMPT_INDEX].ValidTargets.Count);
    }

    [Fact]
    public async Task GivenPlayerInRoom_WhenAttackingOtherPlayerInRoom_ThenFightEndsWithoutAnyErrors()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid playerId = gameDataSeed.PlayerIds.First();

        Exception? exception = await Record.ExceptionAsync(() => Attack(playerId, AttackerStances.ToTheEnd, AttackTypes.Kill));

        Assert.Null(exception);
    }

    [Fact]
    public async Task GivenAttackToTheEnd_WhenFightEnds_ThenDefenderHealthLossIsPersisted()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid attackerId = gameDataSeed.PlayerIds.First();
        Guid defenderId = await GetFirstTargetId(attackerId);

        await Attack(attackerId, AttackerStances.ToTheEnd, AttackTypes.Kill);
        RefreshScope();

        Player defender = await PlayerRepository.GetPlayer(defenderId);
        Assert.True(defender.Health < Player.INITIALIZATION_HEALTH);
    }

    [Fact]
    public async Task GivenAttackToTheEnd_WhenFightEnds_ThenAtLeastOneCombatantIsOutOfTheFight()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid attackerId = gameDataSeed.PlayerIds.First();
        Guid defenderId = await GetFirstTargetId(attackerId);

        await Attack(attackerId, AttackerStances.ToTheEnd, AttackTypes.Kill);
        RefreshScope();

        Player attacker = await PlayerRepository.GetPlayer(attackerId);
        Player defender = await PlayerRepository.GetPlayer(defenderId);
        Assert.Contains(PlayerLifeStatus.Dead, new[] { attacker.LifeStatus, defender.LifeStatus });
    }

    [Fact]
    public async Task GivenStunAttackToTheEnd_WhenFightEnds_ThenNobodyIsDead()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid attackerId = gameDataSeed.PlayerIds.First();
        Guid defenderId = await GetFirstTargetId(attackerId);

        await Attack(attackerId, AttackerStances.ToTheEnd, AttackTypes.Stun);
        RefreshScope();

        Player attacker = await PlayerRepository.GetPlayer(attackerId);
        Player defender = await PlayerRepository.GetPlayer(defenderId);
        Assert.DoesNotContain(PlayerLifeStatus.Dead, new[] { attacker.LifeStatus, defender.LifeStatus });
    }

    [Fact]
    public async Task GivenAttackToTheEnd_WhenFightEnds_ThenDefenderIsToldSomethingInTheirLogs()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid attackerId = gameDataSeed.PlayerIds.First();
        Guid defenderId = await GetFirstTargetId(attackerId);

        await Attack(attackerId, AttackerStances.ToTheEnd, AttackTypes.Kill);
        RefreshScope();

        Player defender = await PlayerRepository.GetPlayer(defenderId);
        Assert.NotEmpty(Context.Entry(defender).Collection(x => x.GameLogs).Query().ToList());
    }

    private async Task<ActionDto> GetCombatAction(Guid playerId)
    {
        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId
        });

        ActionDto actionDto = actions.Actions.First(x => x.Name == ActionNames.CombatAction);

        return actionDto;
    }

    private async Task<Guid> GetFirstTargetId(Guid attackerId)
    {
        ActionDto actionDto = await GetCombatAction(attackerId);
        Guid targetId = actionDto.Prompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets.First().TargetId!.Value;

        return targetId;
    }

    private async Task Attack(Guid attackerId, AttackerStances stance, AttackTypes attackType)
    {
        ActionDto actionDto = await GetCombatAction(attackerId);
        ActionTarget player = actionDto.Prompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets.First();
        ActionTarget attackerStance = actionDto.Prompts[CombatAction.ATTACKERSTANCE_PROMPT_INDEX].ValidTargets
            .First(x => x.Value == stance.ToString());
        ActionTarget attackTypeTarget = actionDto.Prompts[CombatAction.ATTACKTYPE_PROMPT_INDEX].ValidTargets
            .First(x => x.Value == attackType.ToString());

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CombatAction,
            PlayerId = attackerId,
            ActionTargets = [[player], [attackerStance], [attackTypeTarget]]
        });
    }

    // TODO:
    /*
     * What happens when ytou die ?
     * Dead body retrievable, just like a IsDead property ?
     * It's not defined.
     */
}