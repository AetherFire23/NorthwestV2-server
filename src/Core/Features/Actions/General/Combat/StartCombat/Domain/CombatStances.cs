namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// I Remember I had an issue with the naming of this once. It's more an AttackStance or AttackOutcome
/// </summary>
public enum CombatStances
{
    /// <summary>
    /// disengages immediately if Strenght gap is too low, attacker avoids damage and likely avoids identification
    /// </summary>
    HitAndRun,

    /// <summary>
    /// Attacker continues until losing inevitable, then flees, if no kill achieved, they are identified
    /// </summary>
    PushHard,

    /// <summary>
    /// Small Strength boost; attacker refuses to flee - if they lose, they die, or are stunned if defending against stun)
    /// </summary>
    ToTheEnd,
}