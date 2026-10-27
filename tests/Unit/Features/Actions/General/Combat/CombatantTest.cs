using JetBrains.Annotations;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// Toughness: every full Toughness of damage costs 1 Health, the rest carries over within the combat.
/// </summary>
[TestSubject(typeof(Combatant))]
public class CombatantTest
{
    [Fact]
    public void GivenToughnessFour_WhenStruckForTen_ThenLosesTwoHealth()
    {
        Combatant combatant = CreateCombatant(ToughnessInitializationConstants.NORMAL);

        combatant.ReceiveStrike(10, AttackTypes.Kill);

        Assert.Equal(98, combatant.Health);
    }

    [Fact]
    public void GivenLeftoverDamage_WhenStruckAgain_ThenLeftoverAddsToTheNextStrike()
    {
        Combatant combatant = CreateCombatant(ToughnessInitializationConstants.NORMAL);
        combatant.ReceiveStrike(10, AttackTypes.Kill);

        combatant.ReceiveStrike(2, AttackTypes.Kill);

        Assert.Equal(97, combatant.Health);
    }

    [Fact]
    public void GivenStrikeBiggerThanRemainingHealth_WhenStruck_ThenHealthStopsAtZero()
    {
        Combatant combatant = CreateCombatant(ToughnessInitializationConstants.NORMAL);

        combatant.ReceiveStrike(1000, AttackTypes.Kill);

        Assert.Equal(0, combatant.Health);
    }

    [Fact]
    public void GivenStunStrike_WhenStruck_ThenHealthLossIsCountedAsStun()
    {
        Combatant combatant = CreateCombatant(ToughnessInitializationConstants.NORMAL);

        combatant.ReceiveStrike(40, AttackTypes.Stun);

        Assert.Equal(10, combatant.HealthLostToStun);
    }

    [Fact]
    public void GivenModifiersBelowMinusOneHundredPercent_WhenGettingStrength_ThenStrengthIsZero()
    {
        Combatant combatant = CreateCombatant(ToughnessInitializationConstants.NORMAL, meleeModifierPercent: -90);

        decimal strength = combatant.Strength(WeaponRanges.Melee, CombatConstants.HIT_AND_RUN_PERCENT);

        Assert.Equal(0, strength);
    }

    private static Combatant CreateCombatant(int toughness, int meleeModifierPercent = 0)
    {
        Player player = TestFighters.CreateFighter(toughness);

        return new Combatant
        {
            Player = player,
            Toughness = toughness,
            RangedWeapon = null,
            MeleeWeapon = Weapons.Unarmed,
            RangedBasicDamage = 0,
            MeleeBasicDamage = toughness * CombatConstants.UNARMED_DAMAGE_PER_TOUGHNESS,
            RangedModifierPercent = 0,
            MeleeModifierPercent = meleeModifierPercent,
            DamageDealt = AttackTypes.Kill,
            Health = player.Health,
        };
    }
}
