using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.General.Combat.Items;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// What is fixed before the combat: modifiers, weapons, surprise (docs/combat.md, "Strength" and "Combat reactions").
/// </summary>
[TestSubject(typeof(CombatSetupFactory))]
public class CombatSetupFactoryTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;

    private readonly CombatSetupFactory _factory = new CombatSetupFactory();

    [Fact]
    public void GivenWoundedAttacker_WhenSettingUp_ThenHealthPenaltyAppliesToMelee()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.Health = 50;

        CombatSetup setup = Create(attacker, TestFighters.CreateFighter(NORMAL));

        Assert.Equal(-10, setup.Attacker.MeleeModifierPercent);
    }

    [Fact]
    public void GivenWoundedAttacker_WhenSettingUp_ThenHealthPenaltyDoesNotApplyToRanged()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.Health = 50;

        CombatSetup setup = Create(attacker, TestFighters.CreateFighter(NORMAL));

        Assert.Equal(0, setup.Attacker.RangedModifierPercent);
    }

    [Fact]
    public void GivenHopelessLoyal_WhenSettingUp_ThenHeSuffersTheHopePenalty()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.Hope = 10;

        CombatSetup setup = Create(attacker, TestFighters.CreateFighter(NORMAL));

        Assert.Equal(-30, setup.Attacker.RangedModifierPercent);
    }

    [Fact]
    public void GivenHopelessAcolyteFakingNothing_WhenSettingUp_ThenHeFightsAtFullStrength()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.IsAcolyte = true;
        attacker.Hope = 10;
        AttackerChoices choices = new AttackerChoices
        {
            Stance = AttackerStances.PushHard,
            AttackType = AttackTypes.Kill,
            FakedHopePenalty = FakedHopePenalties.None,
        };

        CombatSetup setup = _factory.Create(attacker, TestFighters.CreateFighter(NORMAL), choices, false);

        Assert.Equal(0, setup.Attacker.RangedModifierPercent);
    }

    [Fact]
    public void GivenHopefulAcolyteFakingTheWorstPenalty_WhenSettingUp_ThenHeAppliesIt()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.IsAcolyte = true;
        AttackerChoices choices = new AttackerChoices
        {
            Stance = AttackerStances.PushHard,
            AttackType = AttackTypes.Kill,
            FakedHopePenalty = FakedHopePenalties.Minus30,
        };

        CombatSetup setup = _factory.Create(attacker, TestFighters.CreateFighter(NORMAL), choices, false);

        Assert.Equal(-30, setup.Attacker.RangedModifierPercent);
    }

    [Fact]
    public void GivenAllPenalties_WhenSettingUp_ThenTheyAddUp()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        attacker.Health = 10;
        attacker.Hope = 10;
        attacker.IsWeakened = true;

        CombatSetup setup = Create(attacker, TestFighters.CreateFighter(NORMAL), AttackTypes.Stun);

        Assert.Equal(-100, setup.Attacker.MeleeModifierPercent);
    }

    [Fact]
    public void GivenStunAttackWithAPistol_WhenSettingUp_ThenAttackerIsUnarmed()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL).Carrying(new Pistol());
        AttackerChoices choices = TestFighters.Choices(AttackerStances.PushHard, AttackTypes.Stun, Weapons.Pistol);

        CombatSetup setup = _factory.Create(attacker, TestFighters.CreateFighter(NORMAL), choices, false);

        Assert.False(setup.Attacker.HasRangedWeapon);
    }

    [Fact]
    public void GivenStunAttackAndDefaultReactions_WhenSettingUp_ThenDefenderCountersWithStun()
    {
        Player defender = TestFighters.CreateFighter(NORMAL);

        CombatSetup setup = Create(TestFighters.CreateFighter(NORMAL), defender, AttackTypes.Stun);

        Assert.Equal(AttackTypes.Stun, setup.Defender.DamageDealt);
    }

    [Fact]
    public void GivenStunAttackAndDefaultReactions_WhenSettingUp_ThenDefenderLeavesHisPistolHidden()
    {
        Player defender = TestFighters.CreateFighter(NORMAL).Carrying(new Pistol());

        CombatSetup setup = Create(TestFighters.CreateFighter(NORMAL), defender, AttackTypes.Stun);

        Assert.False(setup.Defender.HasRangedWeapon);
    }

    [Fact]
    public void GivenKillAttackAndDefaultReactions_WhenSettingUp_ThenDefenderUsesHisConcealedPistol()
    {
        Player defender = TestFighters.CreateFighter(NORMAL).Carrying(new Pistol { IsConcealed = true });

        CombatSetup setup = Create(TestFighters.CreateFighter(NORMAL), defender);

        Assert.Equal(Weapons.Pistol, setup.Defender.RangedWeapon);
    }

    [Fact]
    public void GivenReactionNotUsingConcealedWeapons_WhenSettingUp_ThenConcealedPistolStaysHidden()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL).Carrying(new Pistol { IsConcealed = true });
        defender.SetCombatReaction(attacker.Id, AttackTypes.Kill, AttackTypes.Kill,
            WeaponPolicies.StrongestOwnedNotConcealed, FakedHopePenalties.None);

        CombatSetup setup = Create(attacker, defender);

        Assert.False(setup.Defender.HasRangedWeapon);
    }

    [Fact]
    public void GivenStunCounterAgainstAKill_WhenSettingUp_ThenDefenderDealsStunDamage()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.SetCombatReaction(attacker.Id, AttackTypes.Kill, AttackTypes.Stun,
            WeaponPolicies.StrongestOwned, FakedHopePenalties.None);

        CombatSetup setup = Create(attacker, defender);

        Assert.Equal(AttackTypes.Stun, setup.Defender.DamageDealt);
    }

    [Fact]
    public void GivenReactionAgainstAnotherPlayer_WhenSettingUp_ThenDefaultReactionIsUsed()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        Player defender = TestFighters.CreateFighter(NORMAL);
        defender.SetCombatReaction(Guid.NewGuid(), AttackTypes.Kill, AttackTypes.Stun,
            WeaponPolicies.StrongestOwned, FakedHopePenalties.None);

        CombatSetup setup = Create(attacker, defender);

        Assert.Equal(AttackTypes.Kill, setup.Defender.DamageDealt);
    }

    [Fact]
    public void GivenDefenderWithoutVigilance_WhenSettingUp_ThenHeIsSurprised()
    {
        CombatSetup setup = Create(TestFighters.CreateFighter(NORMAL), TestFighters.CreateFighter(NORMAL));

        Assert.True(setup.IsDefenderSurprised);
    }

    [Fact]
    public void GivenVigilantDefender_WhenSettingUp_ThenHeIsNotSurprised()
    {
        Player defender = TestFighters.CreateFighter(NORMAL).Vigilant();

        CombatSetup setup = Create(TestFighters.CreateFighter(NORMAL), defender);

        Assert.False(setup.IsDefenderSurprised);
    }

    [Fact]
    public void GivenSentryWithoutVigilanceSet_WhenSettingUp_ThenHeIsNotSurprised()
    {
        Player sentry = TestFighters.CreateFighter(NORMAL, Roles.Sentry);

        CombatSetup setup = Create(TestFighters.CreateFighter(NORMAL), sentry);

        Assert.False(setup.IsDefenderSurprised);
    }

    private CombatSetup Create(Player attacker, Player defender, AttackTypes attackType = AttackTypes.Kill)
    {
        AttackerChoices choices = TestFighters.Choices(AttackerStances.PushHard, attackType);

        return _factory.Create(attacker, defender, choices, false);
    }
}
