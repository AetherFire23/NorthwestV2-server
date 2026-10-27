using System.ComponentModel.DataAnnotations;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.MyPlayer.Queries;

/// <summary>
/// What a player knows about themselves. Only ever returned to that same player.
/// </summary>
public class GetMyPlayerResponse
{
    [Required] public required Guid Id { get; set; }
    [Required] public required Roles Role { get; set; }
    [Required] public required PlayerLifeStatus LifeStatus { get; set; }

    /// <summary>
    /// Permanent reminder of the player's allegiance, shown the same way to loyal and acolyte players so the
    /// interface itself never betrays anyone. Fellow acolytes are only announced in the game logs (business decision).
    /// </summary>
    [Required] public required bool IsAcolyte { get; set; }

    /// <summary>
    /// 0 to 5. Not even the player knows his exact Health.
    /// </summary>
    [Required] public required int HealthIncrement { get; set; }

    /// <summary>
    /// 0 to 5, secret. For acolytes it is the tracked Hope they should claim.
    /// </summary>
    [Required] public required int HopeIncrement { get; set; }

    /// <summary>
    /// The player's own vigilance setting. The Sentry is always vigilant whatever it says.
    /// </summary>
    [Required] public required bool IsVigilant { get; set; }
}
