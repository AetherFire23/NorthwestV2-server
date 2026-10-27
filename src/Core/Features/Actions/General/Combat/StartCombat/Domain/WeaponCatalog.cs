using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// The weapon table of docs/combat.md, written out. Explosives are not weapons and never appear here.
/// </summary>
public static class WeaponCatalog
{
    public static readonly IReadOnlyList<WeaponProfile> Profiles =
    [
        // Unarmed damage is Toughness x 5, computed in BasicDamage.
        new() { Weapon = Weapons.Unarmed, Range = WeaponRanges.Melee, InsideDamage = 0, OutsideDamage = 0, ItemType = null },
        new() { Weapon = Weapons.Knife, Range = WeaponRanges.Melee, InsideDamage = 40, OutsideDamage = 40, ItemType = ItemTypes.Knife },
        new() { Weapon = Weapons.Sword, Range = WeaponRanges.Melee, InsideDamage = 30, OutsideDamage = 60, ItemType = ItemTypes.Sword },
        new() { Weapon = Weapons.Pistol, Range = WeaponRanges.Ranged, InsideDamage = 100, OutsideDamage = 100, ItemType = ItemTypes.Pistol },
        new() { Weapon = Weapons.Musket, Range = WeaponRanges.Ranged, InsideDamage = 75, OutsideDamage = 175, ItemType = ItemTypes.Musket },
    ];

    /// <summary>
    /// Specialists hit harder with their weapon of choice, inside and outside alike.
    /// </summary>
    private static readonly IReadOnlyList<(Roles Role, Weapons Weapon, decimal Bonus)> SpecialistBonuses =
    [
        (Roles.Brute, Weapons.Unarmed, 15),
        (Roles.Marine, Weapons.Sword, 15),
        (Roles.Marine, Weapons.Musket, 25),
        (Roles.Ranger, Weapons.Musket, 25),
    ];

    public static WeaponProfile Get(Weapons weapon)
    {
        return Profiles.Single(x => x.Weapon == weapon);
    }

    /// <summary>
    /// Damage of one strike before any modifier.
    /// </summary>
    public static decimal BasicDamage(Player player, Weapons weapon, bool isOutside)
    {
        WeaponProfile profile = Get(weapon);

        decimal damage = weapon == Weapons.Unarmed
            ? player.BaseToughness * CombatConstants.UNARMED_DAMAGE_PER_TOUGHNESS
            : isOutside ? profile.OutsideDamage : profile.InsideDamage;

        decimal bonus = SpecialistBonuses
            .Where(x => x.Role == player.Role && x.Weapon == weapon)
            .Sum(x => x.Bonus);

        return damage + bonus;
    }

    /// <summary>
    /// Weapons of that range the player carries. Unarmed is not included.
    /// </summary>
    public static List<Weapons> OwnedWeapons(Player player, WeaponRanges range, bool includeConcealed)
    {
        List<Weapons> owned = player.Inventory.Items
            .Where(item => includeConcealed || !item.IsConcealed)
            .Select(item => Profiles.FirstOrDefault(x => x.ItemType == item.ItemType))
            .Where(profile => profile is not null && profile.Range == range)
            .Select(profile => profile!.Weapon)
            .Distinct()
            .ToList();

        return owned;
    }

    /// <summary>
    /// The candidate with the highest basic damage. Ties keep the earliest candidate, so pass Unarmed first to
    /// avoid revealing a weapon for nothing.
    /// </summary>
    public static Weapons Strongest(Player player, IEnumerable<Weapons> candidates, bool isOutside)
    {
        Weapons strongest = candidates
            .Select((weapon, index) => (weapon, index, damage: BasicDamage(player, weapon, isOutside)))
            .OrderByDescending(x => x.damage)
            .ThenBy(x => x.index)
            .First()
            .weapon;

        return strongest;
    }
}
