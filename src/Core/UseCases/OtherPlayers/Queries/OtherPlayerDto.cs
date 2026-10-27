using System.ComponentModel.DataAnnotations;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.OtherPlayers.Queries;

/// <summary>
/// What a player is allowed to know about another player. Every field is written out on purpose:
/// never add allegiance, exact health, hope, room or inventory here without deciding it is public.
/// Roles are public (all 12 are always in the game). Allegiance is secret, including after death.
/// </summary>
public class OtherPlayerDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required Roles Role { get; set; }

    /// <summary>
    /// Health in 20% increments (0 to 5). Public: its change is the only public trace of a combat.
    /// </summary>
    [Required] public required int HealthIncrement { get; set; }
}
