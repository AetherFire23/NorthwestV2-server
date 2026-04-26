using Mediator;

namespace NorthwestV2.Features.UseCases.GameLogs.Queries;

public class GetGameLogsHandler : IRequestHandler<GetGameLogsRequest, GetGameLogsResponse>
{
    public async ValueTask<GetGameLogsResponse> Handle(GetGameLogsRequest request, CancellationToken cancellationToken)
    {
        
        
        GetGameLogsResponse res = new GetGameLogsResponse()
        {
        };

        return res;
    }
}