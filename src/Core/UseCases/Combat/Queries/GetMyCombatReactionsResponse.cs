using System.ComponentModel.DataAnnotations;

namespace NorthwestV2.UseCases.Combat.Queries;

/// <summary>
/// The player's full reaction matrix: one row per other player and attack type, defaults included.
/// </summary>
public class GetMyCombatReactionsResponse
{
    [Required] public required List<CombatReactionDto> Reactions { get; set; }
}
