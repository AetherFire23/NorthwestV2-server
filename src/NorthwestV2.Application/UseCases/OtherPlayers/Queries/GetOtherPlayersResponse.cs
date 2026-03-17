using System.ComponentModel.DataAnnotations;
using AetherFire23.ERP.Domain.Entity;

namespace NorthwestV2.Application.UseCases.OtherPlayers.Queries;

public class GetOtherPlayersResponse
{
    [Required] public required List<string> PlayerNames { get; set; }
}