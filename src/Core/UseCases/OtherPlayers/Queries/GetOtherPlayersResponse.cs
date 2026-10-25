using System.ComponentModel.DataAnnotations;

namespace NorthwestV2.UseCases.OtherPlayers.Queries;

public class GetOtherPlayersResponse
{
    [Required] public required List<OtherPlayerDto> Players { get; set; }
}
