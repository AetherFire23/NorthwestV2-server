namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Kind of damage a fighter deals: chosen by the attacker, and by the defender as his counter.
/// </summary>
public enum AttackTypes
{
    /// <summary>
    /// Lethal at 0 Health. The only kind of damage weapons deal.
    /// </summary>
    Kill,

    /// <summary>
    /// Always unarmed, -20% Strength, incapacitates at 0 Health. 75% of the Health it took is given back after the combat.
    /// </summary>
    Stun
}
