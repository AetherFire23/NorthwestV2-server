using Mediator;

namespace NorthwestV2.Features.UseCases.GameLogs.Queries;

public class GetGameLogsRequest : IRequest<GetGameLogsResponse>
{
    public required Guid PlayerId { get; set; }
}