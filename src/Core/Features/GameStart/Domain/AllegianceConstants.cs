using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.GameStart.Domain;

/// <summary>
/// Allegiance rules. Both values are public knowledge for the players. See docs/traitors.md.
/// </summary>
/// <remarks>
/// Business decisions (game designer):
/// - A game always has 12 players (one per role), so 2 acolytes is a fixed number and never a proportion.
/// - Players are told how many acolytes exist, but never who they are.
/// - Allegiance is fixed at game creation: no conversion, and it is not revealed when a player dies.
/// </remarks>
public static class AllegianceConstants
{
    public const int AcolyteCount = 2;

    // Public rule: everybody knows the Chaplain is loyal, which gives the table one certain loyal player.
    public static readonly IReadOnlyList<Roles> RolesThatCanNeverBeAcolyte = [Roles.Chaplain];
}
