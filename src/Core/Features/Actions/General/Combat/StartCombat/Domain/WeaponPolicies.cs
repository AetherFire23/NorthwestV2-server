namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Which weapons a defender may use when countering with kill damage.
/// </summary>
public enum WeaponPolicies
{
    StrongestOwned,

    /// <summary>
    /// Leaves concealed weapons hidden, at the cost of fighting with weaker ones.
    /// </summary>
    StrongestOwnedNotConcealed,
}
