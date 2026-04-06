using Mediator;

namespace NorthwestV2.Features.UseCases.OtherPlayers.Queries;

public class GetOtherPlayersRequest : IRequest<GetOtherPlayersResponse>
{
    public required Guid PlayerId { get; set; }
}