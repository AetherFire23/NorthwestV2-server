namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Numeric values behind the combat rules. docs/combat.md names these modifiers without giving numbers,
/// so the values here are design assumptions to confirm.
/// </summary>
public static class CombatConstants
{
    /// <summary>
    /// "To the end -> small Strength boost".
    /// </summary>
    public const int TO_THE_END_STRENGTH_BONUS = 1;

    /// <summary>
    /// "Stun -> weaker Strength" than Kill (Kill is the unmodified baseline).
    /// </summary>
    public const int STUN_STRENGTH_PENALTY = 1;

    /// <summary>
    /// "Override = kill counter (adds Strength ...)".
    /// </summary>
    public const int KILL_COUNTER_STRENGTH_BONUS = 1;
}
