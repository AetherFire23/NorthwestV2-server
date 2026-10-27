using NorthwestV2.Features.Actions.Core.Domain;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// What the attacker picked in the combat prompts.
/// </summary>
public class AttackerChoices
{
    public required AttackerStances Stance { get; init; }
    public required AttackTypes AttackType { get; init; }

    /// <summary>
    /// Null for a melee attack: no distance, no closing in.
    /// </summary>
    public Weapons? RangedWeapon { get; init; }

    public Weapons MeleeWeapon { get; init; } = Weapons.Unarmed;

    /// <summary>
    /// Only read for acolytes.
    /// </summary>
    public FakedHopePenalties FakedHopePenalty { get; init; } = FakedHopePenalties.MatchTrackedHope;
}
