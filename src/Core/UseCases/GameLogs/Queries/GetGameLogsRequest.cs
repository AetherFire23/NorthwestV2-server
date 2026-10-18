using Mediator;

namespace NorthwestV2.UseCases.GameLogs.Queries;

public class GetGameLogsRequest : IRequest<GetGameLogsResponse>
{
    public required Guid PlayerId { get; set; }
}