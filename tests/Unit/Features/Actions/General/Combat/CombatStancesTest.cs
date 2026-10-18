using ERP.Testing.Domain.Dummies;
using JetBrains.Annotations;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Features.Shared.Entity;

namespace ERP.Testing.Domain.Features.Actions.General.Combat;

/// <summary>
/// "Stances" of docs/combat.md: hit-and-run, push hard, to the end.
/// </summary>
[TestSubject(typeof(CombatAction))]
public class CombatStancesTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;
    private const int ANY_LOW_TOUGHNESS = 3;

    private readonly CombatAction _combatAction = new CombatAction();

    [Fact]
    public void GivenToTheEndAttacker_WhenCalculatingStrength_ThenAttackerGetsASmallBoost()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);

        int strength = attacker.CalculateStrength(true, AttackerStances.ToTheEnd);

        Assert.Equal(NORMAL + CombatConstants.TO_THE_END_STRENGTH_BONUS, strength);
    }

    [Fact]
    public void GivenToTheEndStance_WhenCalculatingTheDefenderStrength_ThenDefenderGetsNoBoost()
    {
        Player defender = TestFighters.CreateFighter(NORMAL);

        int strength = defender.CalculateStrength(false, AttackerStances.ToTheEnd);

        Assert.Equal(NORMAL, strength);
    }

    [Fact]
    public void GivenPushHardAttacker_WhenCalculatingStrength_ThenThereIsNoBoost()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);

        int strength = attacker.CalculateStrength(true, AttackerStances.PushHard);

        Assert.Equal(NORMAL, strength);
    }

    [Fact]
    public void GivenHitAndRunAttackerWeakerThanDefender_WhenFighting_ThenTheFightEndsBeforeAnyDamage()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Equal(FightExitType.EarlyExittedBecauseOfHitAndRun, fightResult.FightExitType);
    }

    [Fact]
    public void GivenHitAndRunAttackerWeakerThanDefender_WhenFighting_ThenAttackerAvoidsDamage()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Equal(Player.INITIALIZATION_HEALTH, attacker.Health);
    }

    [Fact]
    public void GivenHitAndRunAttackerWeakerThanDefender_WhenFighting_ThenDefenderIsUnharmed()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Equal(Player.INITIALIZATION_HEALTH, defender.Health);
    }

    [Fact]
    public void GivenHitAndRunAttackerWeakerThanDefender_WhenFighting_ThenAttackerAvoidsIdentification()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.False(fightResult.AttackerIdentified);
    }

    [Fact]
    public void GivenHitAndRunAttackerAsStrongAsDefender_WhenFighting_ThenAttackerStaysAndFights()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.HitAndRun);

        Assert.Equal(FightExitType.ExittedBecauseOfPlayerDeath, fightResult.FightExitType);
    }

    [Fact]
    public void GivenPushHardAttackerLosingTheFight_WhenFighting_ThenAttackerFleesBeforeDying()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.PushHard);

        Assert.Equal(PlayerLifeStatus.Alive, attacker.LifeStatus);
    }

    [Fact]
    public void GivenPushHardAttackerLosingTheFight_WhenFighting_ThenAttackerKeepsTheHealthLeftWhenFleeing()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.PushHard);

        // Loses 12 a round and flees as soon as the next blow would be fatal: 100 - 8 * 12.
        Assert.Equal(4, attacker.Health);
    }

    [Fact]
    public void GivenPushHardAttackerLosingTheFight_WhenFighting_ThenDefenderSurvivesTheAttack()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.PushHard);

        Assert.Equal(PlayerLifeStatus.Alive, defender.LifeStatus);
    }

    [Fact]
    public void GivenPushHardAttackerFleeingWithoutAKill_WhenFighting_ThenAttackerIsIdentified()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.PushHard);

        Assert.True(fightResult.AttackerIdentified);
    }

    [Fact]
    public void GivenPushHardAttackerWhoWins_WhenFighting_ThenAttackerDoesNotFlee()
    {
        Player attacker = TestFighters.CreateFighter(ToughnessInitializationConstants.TOUGH);
        Player defender = TestFighters.CreateFighter(ToughnessInitializationConstants.WEAK);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.PushHard);

        Assert.Equal(FightExitType.ExittedBecauseOfPlayerDeath, fightResult.FightExitType);
    }

    [Fact]
    public void GivenToTheEndAttackerLosingTheFight_WhenFighting_ThenAttackerRefusesToFlee()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(FightExitType.ExittedBecauseOfPlayerDeath, fightResult.FightExitType);
    }

    [Fact]
    public void GivenToTheEndAttackerLosingTheFight_WhenFighting_ThenAttackerDies()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.Equal(PlayerLifeStatus.Dead, attacker.LifeStatus);
    }

    [Fact]
    public void GivenToTheEndAttackerLosingTheFight_WhenFighting_ThenAttackerIsIdentified()
    {
        Player attacker = TestFighters.CreateFighter(ANY_LOW_TOUGHNESS);
        Player defender = TestFighters.CreateFighter(NORMAL);

        FightResult fightResult = _combatAction.MakeTwoPlayerFightTogether(attacker, defender, AttackerStances.ToTheEnd);

        Assert.True(fightResult.AttackerIdentified);
    }
}
