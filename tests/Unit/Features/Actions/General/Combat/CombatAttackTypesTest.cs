using ERP.Testing.Domain.Dummies;
using JetBrains.Annotations;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Features.Shared.Entity;

namespace ERP.Testing.Domain.Features.Actions.General.Combat;

/// <summary>
/// "Attack types" and "Defensive counters" of docs/combat.md.
/// Numeric modifiers are the assumptions listed in <see cref="CombatConstants"/>.
/// </summary>
[TestSubject(typeof(CombatAction))]
public class CombatAttackTypesTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;
    private const int WEAK = ToughnessInitializationConstants.WEAK;

    private readonly CombatAction _combatAction = new CombatAction();

    [Fact]
    public void GivenKillAttack_WhenCalculatingAttackerStrength_ThenStrengthIsTheToughness()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.AttackTypes = AttackTypes.Kill;

        int strength = attacker.CalculateStrength(true, AttackerStances.PushHard);

        Assert.Equal(NORMAL, strength);
    }

    [Fact]
    public void GivenStunAttack_WhenCalculatingAttackerStrength_ThenStrengthIsWeakerThanAKill()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.AttackTypes = AttackTypes.Stun;

        int strength = attacker.CalculateStrength(true, AttackerStances.PushHard);

        Assert.Equal(NORMAL - CombatConstants.STUN_STRENGTH_PENALTY, strength);
    }

    [Fact]
    public void GivenStunAttackFromAToughnessOfOne_WhenCalculatingAttackerStrength_ThenStrengthNeverDropsBelowOne()
    {
        Player attacker = TestFighters.CreateFighter(1);
        attacker.AttackTypes = AttackTypes.Stun;

        int strength = attacker.CalculateStrength(true, AttackerStances.PushHard);

        Assert.Equal(1, strength);
    }

    [Fact]
    public void GivenDefaultCounter_WhenCalculatingDefenderStrength_ThenStrengthIsTheToughness()
    {
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.DefensiveCounter = DefensiveCounters.DefaultVsStun;

        int strength = defender.CalculateStrength(false, AttackerStances.PushHard);

        Assert.Equal(NORMAL, strength);
    }

    [Fact]
    public void GivenKillCounter_WhenCalculatingDefenderStrength_ThenStrengthGainsTheCounterBonus()
    {
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.DefensiveCounter = DefensiveCounters.Override;

        int strength = defender.CalculateStrength(false, AttackerStances.PushHard);

        Assert.Equal(NORMAL + CombatConstants.KILL_COUNTER_STRENGTH_BONUS, strength);
    }

    [Fact]
    public void GivenKillCounterChosenEarlier_WhenCalculatingStrengthAsTheAttacker_ThenCounterBonusDoesNotApply()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.DefensiveCounter = DefensiveCounters.Override;

        int strength = attacker.CalculateStrength(true, AttackerStances.PushHard);

        Assert.Equal(NORMAL, strength);
    }

    [Fact]
    public void GivenKillAttackerWhoLoses_WhenFighting_ThenAttackerIsDead()
    {
        Player attacker = TestFighters.CreateFighter(WEAK);
        attacker.AttackTypes = AttackTypes.Kill;
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Dead, attacker.LifeStatus);
    }

    [Fact]
    public void GivenKillAttackerWhoWins_WhenFighting_ThenDefenderIsDead()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.AttackTypes = AttackTypes.Kill;
        Player defender = TestFighters.CreateFighter(WEAK);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Dead, defender.LifeStatus);
    }

    [Fact]
    public void GivenStunAttackerWhoWins_WhenFighting_ThenDefenderIsStunnedNotDead()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.AttackTypes = AttackTypes.Stun;
        Player defender = TestFighters.CreateFighter(WEAK);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Stunned, defender.LifeStatus);
    }

    [Fact]
    public void GivenStunAttackerWhoWins_WhenFighting_ThenDefenderIsIncapacitatedAtZeroHealth()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.AttackTypes = AttackTypes.Stun;
        Player defender = TestFighters.CreateFighter(WEAK);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(0, defender.Health);
    }

    [Fact]
    public void GivenStunAttackerWhoWins_WhenFighting_ThenDefenderIsNotTheDeadPlayer()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.AttackTypes = AttackTypes.Stun;
        Player defender = TestFighters.CreateFighter(WEAK);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Null(fightResult.DeadPlayer);
    }

    [Fact]
    public void GivenStunAttackerWhoLosesToTheEndAgainstTheDefaultCounter_WhenFighting_ThenAttackerIsStunnedNotDead()
    {
        Player attacker = TestFighters.CreateFighter(WEAK);
        attacker.AttackTypes = AttackTypes.Stun;
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.DefensiveCounter = DefensiveCounters.DefaultVsStun;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Stunned, attacker.LifeStatus);
    }

    [Fact]
    public void GivenStunAttackerWhoLosesToTheEndAgainstTheKillCounter_WhenFighting_ThenAttackerIsDead()
    {
        Player attacker = TestFighters.CreateFighter(WEAK);
        attacker.AttackTypes = AttackTypes.Stun;
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.DefensiveCounter = DefensiveCounters.Override;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Dead, attacker.LifeStatus);
    }

    [Fact]
    public void GivenKillAttackerWhoLosesAgainstTheDefaultCounter_WhenFighting_ThenAttackerIsDead()
    {
        Player attacker = TestFighters.CreateFighter(WEAK);
        attacker.AttackTypes = AttackTypes.Kill;
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.DefensiveCounter = DefensiveCounters.DefaultVsStun;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Dead, attacker.LifeStatus);
    }

    [Fact]
    public void GivenKillCounterAgainstAStun_WhenFighting_ThenDefenderStrengthInTheFightIncludesTheCounterBonus()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.AttackTypes = AttackTypes.Stun;
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.DefensiveCounter = DefensiveCounters.Override;

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(NORMAL + CombatConstants.KILL_COUNTER_STRENGTH_BONUS, fightResult.DefenderFightStats.Strength);
    }
}
