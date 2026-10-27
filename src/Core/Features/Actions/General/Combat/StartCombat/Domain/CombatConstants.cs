namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Numbers behind the combat rules (docs/combat.md). Percentages are Strength modifiers applied to the basic damage.
/// </summary>
public static class CombatConstants
{
    public const int UNARMED_DAMAGE_PER_TOUGHNESS = 5;

    public const int TO_THE_END_PERCENT = 10;
    public const int HIT_AND_RUN_PERCENT = -20;
    public const int STUN_PERCENT = -20;
    public const int WEAKENED_PERCENT = -30;

    /// <summary>
    /// A surprise attack in melee (no ranged weapon used) hits twice as hard.
    /// </summary>
    public const int SURPRISE_MELEE_MULTIPLIER = 2;

    /// <summary>
    /// Push hard breaks off at this Health or less, if he would lose.
    /// </summary>
    public const int PUSH_HARD_BREAK_OFF_HEALTH = 20;

    /// <summary>
    /// Hit-and-run stops early if the attacker's Health would drop to this or less.
    /// </summary>
    public const int HIT_AND_RUN_STOP_HEALTH = 40;

    /// <summary>
    /// Share of the Health lost to stun damage that is given back at the end of the combat, rounded up.
    /// </summary>
    public const decimal STUN_HEALTH_REFUND_RATIO = 0.75m;

    /// <summary>
    /// Hit-and-run stopped after a surprise attack: chance (in %) that the defender identifies the attacker.
    /// </summary>
    public const int IDENTIFICATION_PERCENT_AFTER_SURPRISE = 20;

    /// <summary>
    /// Hit-and-run stopped with no damage because the defender was not surprised.
    /// </summary>
    public const int IDENTIFICATION_PERCENT_WITHOUT_SURPRISE = 60;

    /// <summary>
    /// Safety net: a combat that has not ended after this many rounds is a bug, not a long fight.
    /// </summary>
    public const int MAX_ROUNDS = 10_000;
}
