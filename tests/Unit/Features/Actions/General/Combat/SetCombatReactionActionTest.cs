using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.SetCombatReaction;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

[TestSubject(typeof(SetCombatReactionAction))]
public class SetCombatReactionActionTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;

    private readonly SetCombatReactionAction _action = new SetCombatReactionAction();

    [Fact]
    public void GivenNoReactionSet_WhenReadingIt_ThenDefaultCountersAKillWithAKill()
    {
        Player player = TestFighters.CreateFighter(NORMAL);

        CombatReaction reaction = player.GetCombatReaction(Guid.NewGuid(), AttackTypes.Kill);

        Assert.Equal(AttackTypes.Kill, reaction.Counter);
    }

    [Fact]
    public void GivenNoReactionSet_WhenReadingTheAcolyteDefaultAgainstAStun_ThenHeFakesHisTrackedHope()
    {
        Player player = TestFighters.CreateFighter(NORMAL);

        CombatReaction reaction = player.GetCombatReaction(Guid.NewGuid(), AttackTypes.Stun);

        Assert.Equal(FakedHopePenalties.MatchTrackedHope, reaction.FakedHopePenalty);
    }

    [Fact]
    public void GivenLoyal_WhenGettingAvailability_ThenThereIsNoFakedHopePrompt()
    {
        Player player = TestFighters.CreateFighter(NORMAL);

        ActionWithTargetsAvailability availability = _action.DetermineAvailability(player, []);

        Assert.Equal(SetCombatReactionAction.FAKED_HOPE_PENALTY_PROMPT_INDEX, availability.TargetSelectionPrompts.Count);
    }

    [Fact]
    public void GivenStunCounterChosen_WhenExecuting_ThenReactionAgainstThatOpponentIsStored()
    {
        Player player = TestFighters.CreateFighter(NORMAL);
        Guid opponentId = Guid.NewGuid();

        _action.Execute(player, CreateTargets(opponentId, AttackTypes.Kill, AttackTypes.Stun));

        Assert.Equal(AttackTypes.Stun, player.GetCombatReaction(opponentId, AttackTypes.Kill).Counter);
    }

    [Fact]
    public void GivenReactionSetTwice_WhenExecuting_ThenOnlyOneReactionIsStored()
    {
        Player player = TestFighters.CreateFighter(NORMAL);
        Guid opponentId = Guid.NewGuid();
        _action.Execute(player, CreateTargets(opponentId, AttackTypes.Kill, AttackTypes.Stun));

        _action.Execute(player, CreateTargets(opponentId, AttackTypes.Kill, AttackTypes.Kill));

        Assert.Single(player.CombatReactions);
    }

    private static List<List<ActionTarget>> CreateTargets(Guid opponentId, AttackTypes attackType, AttackTypes counter)
    {
        return
        [
            [new ActionTarget { TargetId = opponentId }],
            [new ActionTarget { Value = attackType.ToString() }],
            [new ActionTarget { Value = counter.ToString() }],
            [new ActionTarget { Value = nameof(WeaponPolicies.StrongestOwned) }],
        ];
    }
}
