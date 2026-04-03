namespace AetherFire23.ERP.Domain.Features.Actions.Core;

/// <summary>
/// Names for the descriptions. Also will be mapped to the executed action on the frontend.
///
/// // TODO: Use nameof() one day to make a distinction between an action ID and an action display name.
/// Action Id would map api -> service 
/// </summary>
public static class ActionNames
{
    public const string InstantHeal = "Instant Heal";
    public const string DebugWithTargets = "Debug action with targets";
    public const string PickDefensiveStance = "Choose defensive stance";
    public const string CombatAction = "Combat";
    public const string ChangeRoom = "Change Room";

    public const string SpyglassProductionStart = "Start Spyglass Production";
    public const string SpyglassProductionSecond = "Secon dstage production";
    public const string SpyglassContribution = "SpyglassContribution";

    // public const string SpyglassProduction = "Spyglass Production";
    public static string HammerProductionInitiation => "Initiate hammer production";
    public static string CancelProduction => "Cancel a production";
    public const string HammerProductionContribution = "HammerProductionContribution";
}