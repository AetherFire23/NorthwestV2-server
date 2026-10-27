namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Hope Strength penalty an acolyte applies to himself so that his fighting matches the Hope he claims.
/// Acolytes are never afflicted by a real Hope penalty. Ignored for loyals.
/// </summary>
public enum FakedHopePenalties
{
    /// <summary>
    /// The penalty a loyal would have at the acolyte's tracked Hope.
    /// </summary>
    MatchTrackedHope,

    /// <summary>
    /// Full Strength.
    /// </summary>
    None,

    /// <summary>
    /// The 21-40% Hope penalty.
    /// </summary>
    Minus10,

    /// <summary>
    /// The 1-20% Hope penalty.
    /// </summary>
    Minus30,
}
