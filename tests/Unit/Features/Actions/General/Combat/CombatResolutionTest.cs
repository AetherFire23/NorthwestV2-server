using ERP.Testing.Domain.Dummies;
using JetBrains.Annotations;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Features.Shared.Entity;

namespace ERP.Testing.Domain.Features.Actions.General.Combat;

/// <summary>
/// "Combat Core" and "Persistence" of docs/combat.md.
/// </summary>
[TestSubject(typeof(CombatAction))]
public class CombatResolutionTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;

    private readonly CombatAction _combatAction = new CombatAction();

    [Fact]
    public void GivenAttackerOneStrengthAheadWithToTheEnd_WhenFighting_ThenAttackerKeepsTheHealthLeftByTheRounds()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        // Attacker hits for 13, defender for 12. Defender is out after 8 rounds: 100 - 8 * 12 = 4.
        Assert.Equal(4, attacker.Health);
    }

    [Fact]
    public void GivenAttackerOneStrengthAheadWithToTheEnd_WhenFighting_ThenDefenderHealthIsZeroNotNegative()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(0, defender.Health);
    }

    [Fact]
    public void GivenAttackerOneStrengthAheadWithToTheEnd_WhenFighting_ThenDefenderIsDead()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Dead, defender.LifeStatus);
    }

    [Fact]
    public void GivenAttackerOneStrengthAheadWithToTheEnd_WhenFighting_ThenAttackerIsStillAlive()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Alive, attacker.LifeStatus);
    }

    [Fact]
    public void GivenAttackerOneStrengthAheadWithToTheEnd_WhenFighting_ThenAttackerIsTheWinner()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.True(fightResult.IsWinner(attacker));
    }

    [Fact]
    public void GivenAttackerOneStrengthAheadWithToTheEnd_WhenFighting_ThenDefenderIsTheDeadPlayer()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Same(defender, fightResult.DeadPlayer!.Player);
    }

    [Fact]
    public void GivenAttackerOneStrengthAheadWithToTheEnd_WhenFighting_ThenBothCombatantsAreInTheResult()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Same(defender, fightResult.DefenderFightStats.Player);
    }

    [Fact]
    public void GivenDefenderWithExactlyEnoughHealthToLoseAtZero_WhenFighting_ThenDefenderIsDeadAtZeroHealth()
    {
        Player attacker = TestFighters.CreateFighter(10);
        Player defender = TestFighters.CreateFighter(10);
        defender.Health = 10;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Equal(PlayerLifeStatus.Dead, defender.LifeStatus);
    }

    [Fact]
    public void GivenAttackerWithExactlyEnoughHealthToLoseAtZero_WhenFighting_ThenAttackerIsNotDeclaredWinner()
    {
        Player attacker = TestFighters.CreateFighter(10);
        Player defender = TestFighters.CreateFighter(10);
        attacker.Health = 10;

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.False(fightResult.IsWinner(attacker));
    }

    [Fact]
    public void GivenTwoEquallyMatchedPlayers_WhenBothReachZeroHealthInTheSameRound_ThenNobodySurvives()
    {
        Player attacker = TestFighters.CreateFighter(10);
        Player defender = TestFighters.CreateFighter(10);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Null(fightResult.SurvivingPlayer);
    }

    [Fact]
    public void GivenTwoEquallyMatchedPlayers_WhenBothReachZeroHealthInTheSameRound_ThenAttackerIsDead()
    {
        Player attacker = TestFighters.CreateFighter(10);
        Player defender = TestFighters.CreateFighter(10);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Equal(PlayerLifeStatus.Dead, attacker.LifeStatus);
    }

    [Fact]
    public void GivenTwoEquallyMatchedPlayers_WhenBothReachZeroHealthInTheSameRound_ThenDefenderIsDead()
    {
        Player attacker = TestFighters.CreateFighter(10);
        Player defender = TestFighters.CreateFighter(10);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Equal(PlayerLifeStatus.Dead, defender.LifeStatus);
    }

    [Fact]
    public void GivenTwoEquallyMatchedPlayers_WhenBothReachZeroHealthInTheSameRound_ThenThereIsNoSingleDeadPlayer()
    {
        Player attacker = TestFighters.CreateFighter(10);
        Player defender = TestFighters.CreateFighter(10);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Null(fightResult.DeadPlayer);
    }

    [Fact]
    public void GivenStrengthBelowToughness_WhenFighting_ThenToughnessDepletionCarriesOverBetweenRounds()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.Sanity = 49; // Strength 11 against a toughness of 12: no damage in round 1.
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.Health = 11;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.PushHard);

        // Defender goes down in round 2, the attacker lost 12 in each of the 2 rounds: 100 - 24.
        Assert.Equal(76, attacker.Health);
    }

    [Fact]
    public void GivenAnyFight_WhenFightEnds_ThenBaseToughnessIsUntouchedForTheNextFight()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(NORMAL, attacker.BaseToughness);
    }

    [Fact]
    public void GivenAttackerHurtInAPreviousFight_WhenFightingAnEquallyMatchedDefender_ThenHurtAttackerGoesDownFirst()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);
        attacker.Health = 50;

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        // Even one Strength ahead, 50 Health is gone after 5 rounds of 12, while the defender still has 35.
        Assert.True(fightResult.IsWinner(defender));
    }
}
