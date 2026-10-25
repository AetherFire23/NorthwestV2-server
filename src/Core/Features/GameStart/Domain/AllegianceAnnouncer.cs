using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.GameStart.Domain;

/// <summary>
/// Gives every player a private log telling them their allegiance at game start.
/// Acolytes also learn who the other acolytes are; the log is the only place that information lives.
/// </summary>
/// <remarks>
/// Business decisions (game designer):
/// - Loyal players get a log too: if only acolytes received one, having no log would betray a loyal player.
/// - The fellow acolyte is given in full (username and role), not just the name.
/// - The co-acolyte is deliberately not exposed by any DTO or query, so the start-of-game log is the only source.
/// - Messages are in English like every other game log. The dramatic announcement (text and image) is frontend-only.
/// </remarks>
public class AllegianceAnnouncer
{
    public void AnnounceAllegiances(IReadOnlyCollection<Player> players)
    {
        foreach (Player player in players)
        {
            player.GameLogs.Add(new GameLog
            {
                Message = player.IsAcolyte
                    ? DescribeAcolyteAllegiance(player, players)
                    : "You are loyal."
            });
        }
    }

    private static string DescribeAcolyteAllegiance(Player acolyte, IEnumerable<Player> players)
    {
        // The acolyte count is fixed (see AllegianceConstants), so this joins one name today, but it stays a list on purpose.
        string fellowAcolytes = string.Join(", ", players
            // Reference comparison: ids are still empty before the first save.
            .Where(x => x.IsAcolyte && !ReferenceEquals(x, acolyte))
            .Select(x => $"{x.User.Username} ({x.Role})"));

        return $"You are an acolyte. Your fellow acolyte: {fellowAcolytes}.";
    }
}
