using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Basic damage of a weapon, before modifiers. <see cref="ItemType"/> is null for <see cref="Weapons.Unarmed"/>,
/// whose damage depends on the fighter's Toughness instead.
/// </summary>
public class WeaponProfile
{
    public required Weapons Weapon { get; init; }
    public required WeaponRanges Range { get; init; }
    public required decimal InsideDamage { get; init; }
    public required decimal OutsideDamage { get; init; }
    public required ItemTypes? ItemType { get; init; }
}
