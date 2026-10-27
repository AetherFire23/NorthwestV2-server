using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.Items;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// Who may attack whom, and the attacker's prompts.
/// </summary>
[TestSubject(typeof(CombatAction))]
public class CombatActionTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;

    private readonly CombatAction _combatAction = new CombatAction(new CombatSetupFactory(), new CombatResolver());

    [Fact]
    public void GivenPlayerAndOtherPlayerInSameRoom_WhenGettingAvailability_ThenCanSeeOtherPlayer()
    {
        Player caster = TestFighters.CreateFighter(NORMAL);
        Player target = TestFighters.CreateFighter(NORMAL, Roles.QuarterMaster);

        ActionWithTargetsAvailability availability = _combatAction.DetermineAvailability(caster, [target]);

        Assert.Equal(target.Id, availability.TargetSelectionPrompts[CombatAction.PLAYER_PROMPT_INDEX]
            .ValidTargets.Single().TargetId);
    }

    [Fact]
    public void GivenDeadPlayerInRoom_WhenGettingAvailability_ThenHeCannotBeTargeted()
    {
        Player caster = TestFighters.CreateFighter(NORMAL);
        Player target = TestFighters.CreateFighter(NORMAL, Roles.QuarterMaster);
        target.LifeStatus = PlayerLifeStatus.Dead;

        ActionWithTargetsAvailability availability = _combatAction.DetermineAvailability(caster, [target]);

        Assert.Empty(availability.TargetSelectionPrompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets);
    }

    [Fact]
    public void GivenChaplain_WhenGettingAvailability_ThenHeCannotAttack()
    {
        Player chaplain = TestFighters.CreateFighter(ToughnessInitializationConstants.WEAK, Roles.Chaplain);
        Player target = TestFighters.CreateFighter(NORMAL);

        ActionWithTargetsAvailability availability = _combatAction.DetermineAvailability(chaplain, [target]);

        Assert.False(availability.CanExecute);
    }

    [Fact]
    public void GivenStunnedPlayer_WhenGettingAvailability_ThenHeCannotAttack()
    {
        Player caster = TestFighters.CreateFighter(NORMAL);
        caster.LifeStatus = PlayerLifeStatus.Stunned;
        Player target = TestFighters.CreateFighter(NORMAL);

        ActionWithTargetsAvailability availability = _combatAction.DetermineAvailability(caster, [target]);

        Assert.False(availability.CanExecute);
    }

    [Fact]
    public void GivenAliveTargetInRoom_WhenGettingAvailability_ThenCanAttack()
    {
        Player caster = TestFighters.CreateFighter(NORMAL);
        Player target = TestFighters.CreateFighter(NORMAL);

        ActionWithTargetsAvailability availability = _combatAction.DetermineAvailability(caster, [target]);

        Assert.True(availability.CanExecute);
    }

    [Fact]
    public void GivenLoyal_WhenGettingAvailability_ThenThereIsNoFakedHopePrompt()
    {
        Player caster = TestFighters.CreateFighter(NORMAL);

        ActionWithTargetsAvailability availability =
            _combatAction.DetermineAvailability(caster, [TestFighters.CreateFighter(NORMAL)]);

        Assert.Equal(CombatAction.FAKED_HOPE_PENALTY_PROMPT_INDEX, availability.TargetSelectionPrompts.Count);
    }

    [Fact]
    public void GivenAcolyte_WhenGettingAvailability_ThenHeCanPickAFakedHopePenalty()
    {
        Player caster = TestFighters.CreateFighter(NORMAL);
        caster.IsAcolyte = true;

        ActionWithTargetsAvailability availability =
            _combatAction.DetermineAvailability(caster, [TestFighters.CreateFighter(NORMAL)]);

        Assert.Equal(Enum.GetValues<FakedHopePenalties>().Length,
            availability.TargetSelectionPrompts[CombatAction.FAKED_HOPE_PENALTY_PROMPT_INDEX].ValidTargets.Count);
    }

    [Fact]
    public void GivenCaptainLoadout_WhenGettingAvailability_ThenRangedChoicesAreNoneOrHisPistol()
    {
        Player captain = TestFighters.CreateFighter(NORMAL, Roles.Captain).Carrying(new Sword(), new Pistol());

        ActionWithTargetsAvailability availability =
            _combatAction.DetermineAvailability(captain, [TestFighters.CreateFighter(NORMAL)]);

        Assert.Equal([CombatAction.NO_RANGED_WEAPON, nameof(Weapons.Pistol)],
            availability.TargetSelectionPrompts[CombatAction.RANGED_WEAPON_PROMPT_INDEX].ValidTargets
                .Select(x => x.Value));
    }

    [Fact]
    public void GivenCaptainLoadout_WhenGettingAvailability_ThenMeleeChoicesAreBareHandsOrHisSword()
    {
        Player captain = TestFighters.CreateFighter(NORMAL, Roles.Captain).Carrying(new Sword(), new Pistol());

        ActionWithTargetsAvailability availability =
            _combatAction.DetermineAvailability(captain, [TestFighters.CreateFighter(NORMAL)]);

        Assert.Equal([nameof(Weapons.Unarmed), nameof(Weapons.Sword)],
            availability.TargetSelectionPrompts[CombatAction.MELEE_WEAPON_PROMPT_INDEX].ValidTargets
                .Select(x => x.Value));
    }

    [Fact]
    public void GivenNoRangedWeaponPicked_WhenReadingChoices_ThenItIsAMeleeAttack()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        List<List<ActionTarget>> targets = CreateTargets(CombatAction.NO_RANGED_WEAPON);

        AttackerChoices choices = _combatAction.DetermineAttackerChoices(targets, attacker);

        Assert.Null(choices.RangedWeapon);
    }

    [Fact]
    public void GivenPistolPicked_WhenReadingChoices_ThenPistolIsTheRangedWeapon()
    {
        Player attacker = TestFighters.CreateFighter(NORMAL);
        List<List<ActionTarget>> targets = CreateTargets(nameof(Weapons.Pistol));

        AttackerChoices choices = _combatAction.DetermineAttackerChoices(targets, attacker);

        Assert.Equal(Weapons.Pistol, choices.RangedWeapon);
    }

    private static List<List<ActionTarget>> CreateTargets(string rangedWeapon)
    {
        return
        [
            [new ActionTarget { TargetId = Guid.NewGuid() }],
            [new ActionTarget { Value = nameof(AttackerStances.PushHard) }],
            [new ActionTarget { Value = nameof(AttackTypes.Kill) }],
            [new ActionTarget { Value = rangedWeapon }],
            [new ActionTarget { Value = nameof(Weapons.Unarmed) }],
        ];
    }
}
