namespace NorthwestV2.Features.GameStart.Domain.RoleInitializations;

/// <summary>
/// Starting toughness per role (docs/combat.md). Each time the damage a fighter takes adds up to his toughness,
/// he loses 1 Health. Public and never changes.
/// </summary>
public class ToughnessInitializationConstants
{
    /// <summary>
    /// Chaplain, Scholar, Doctor.
    /// </summary>
    public const int WEAK = 3;

    public const int NORMAL = 4;

    /// <summary>
    /// Marine, Brute, Sapper.
    /// </summary>
    public const int TOUGH = 5;
}
