using Mediator;

namespace NorthwestV2.UseCases.MyPlayer.Queries;

public class GetMyPlayerRequest : IRequest<GetMyPlayerResponse>
{
    public required Guid PlayerId { get; set; }
}
