namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

public enum DefensiveCounters
{
    /// <summary>
    /// Counter-attack in kind: non-lethal against a stun, lethal against a kill.
    /// </summary>
    DefaultVsStun,

    /// <summary>
    /// Kill counter: adds Strength, and is lethal even against a stun (a non-lethal attacker risks dying).
    /// </summary>
    Override,
}