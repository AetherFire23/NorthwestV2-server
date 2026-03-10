namespace AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;

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