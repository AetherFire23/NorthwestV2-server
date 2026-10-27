namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// What a fighter strikes with. <see cref="Unarmed"/> is always available; the others require the matching item.
/// </summary>
public enum Weapons
{
    Unarmed,
    Knife,
    Sword,
    Pistol,
    Musket,
}
