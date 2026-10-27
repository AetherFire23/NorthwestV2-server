namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

public enum FightExitType
{
    /// <summary>
    /// One fighter reached 0 Health.
    /// </summary>
    Knockout,

    /// <summary>
    /// Push hard attacker at 20 Health or less who would lose.
    /// </summary>
    BrokeOffUnderPushHard,

    /// <summary>
    /// The attacker had 0 Strength when the rounds were reached, whatever his stance.
    /// </summary>
    BrokeOffWithoutStrength,

    /// <summary>
    /// Hit-and-run stopped after the surprise attack (or the lack of one): no closing in, no rounds.
    /// </summary>
    StoppedByHitAndRun,
}
