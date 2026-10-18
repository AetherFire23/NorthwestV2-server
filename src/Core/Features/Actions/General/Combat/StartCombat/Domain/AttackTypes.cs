namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

public enum AttackTypes
{
    /// <summary>
    ///  higher Strength, lethal at 0 Health
    /// </summary>
    Kill,
    
    /// <summary>
    /// weaker Strength, incapacitates at 0 Health
    /// </summary>
    Stun
}