using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// One fighter during a single combat. Everything except Health and life status is discarded when the combat ends.
/// Modifiers are fixed when the combat starts (<see cref="CombatSetupFactory"/>); damage taken never changes them.
/// </summary>
public class Combatant
{
    public required Player Player { get; init; }
    public required int Toughness { get; init; }

    /// <summary>
    /// Null when the fighter does not use a ranged weapon in this combat.
    /// </summary>
    public required Weapons? RangedWeapon { get; init; }

    public required Weapons MeleeWeapon { get; init; }
    public required decimal RangedBasicDamage { get; init; }
    public required decimal MeleeBasicDamage { get; init; }

    /// <summary>
    /// Sum of the fixed percentages for ranged strikes: Hope, weakness, stun.
    /// </summary>
    public required int RangedModifierPercent { get; init; }

    /// <summary>
    /// Sum of the fixed percentages for melee strikes: the ranged ones plus Health.
    /// </summary>
    public required int MeleeModifierPercent { get; init; }

    public required AttackTypes DamageDealt { get; init; }

    public required int Health { get; set; }

    /// <summary>
    /// Damage received that has not yet added up to a full Toughness.
    /// </summary>
    public decimal LeftoverDamage { get; set; }

    public int HealthLostToStun { get; set; }

    /// <summary>
    /// Kind of damage that took this fighter to 0 Health, if it happened.
    /// </summary>
    public AttackTypes? DownedBy { get; set; }

    public List<Weapons> WeaponsUsed { get; init; } = [];

    public bool HasRangedWeapon => RangedWeapon.HasValue;
    public bool IsDown => Health <= 0;

    /// <param name="phasePercent">Stance modifier, only non-zero for the attacker during rounds.</param>
    public decimal Strength(WeaponRanges range, int phasePercent)
    {
        bool isRanged = range == WeaponRanges.Ranged;
        decimal basicDamage = isRanged ? RangedBasicDamage : MeleeBasicDamage;
        int totalPercent = (isRanged ? RangedModifierPercent : MeleeModifierPercent) + phasePercent;

        decimal strength = basicDamage * (100 + totalPercent) / 100m;

        return Math.Max(0, strength);
    }

    public Weapons WeaponFor(WeaponRanges range)
    {
        return range == WeaponRanges.Ranged
            ? RangedWeapon ?? throw new InvalidOperationException($"{Player} has no ranged weapon in this combat.")
            : MeleeWeapon;
    }

    /// <summary>
    /// Every full Toughness of accumulated damage costs 1 Health. The rest carries over to the next strike.
    /// </summary>
    public void ReceiveStrike(decimal damage, AttackTypes damageType)
    {
        LeftoverDamage += damage;

        int healthLost = (int)Math.Floor(LeftoverDamage / Toughness);
        LeftoverDamage -= healthLost * Toughness;
        healthLost = Math.Min(healthLost, Health);

        Health -= healthLost;

        if (damageType == AttackTypes.Stun)
        {
            HealthLostToStun += healthLost;
        }

        if (IsDown)
        {
            DownedBy ??= damageType;
        }
    }

    public Combatant Clone()
    {
        Combatant clone = new Combatant
        {
            Player = Player,
            Toughness = Toughness,
            RangedWeapon = RangedWeapon,
            MeleeWeapon = MeleeWeapon,
            RangedBasicDamage = RangedBasicDamage,
            MeleeBasicDamage = MeleeBasicDamage,
            RangedModifierPercent = RangedModifierPercent,
            MeleeModifierPercent = MeleeModifierPercent,
            DamageDealt = DamageDealt,
            Health = Health,
            LeftoverDamage = LeftoverDamage,
            HealthLostToStun = HealthLostToStun,
            DownedBy = DownedBy,
            WeaponsUsed = [..WeaponsUsed],
        };

        return clone;
    }

    public override string ToString()
    {
        return $"{Player.Role} ({Health} Health)";
    }
}
