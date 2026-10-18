using Mediator;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.UseCases.GameLogs.Queries;

public class GetGameLogsHandler : IRequestHandler<GetGameLogsRequest, GetGameLogsResponse>
{
    private readonly IGameLogsRepository _gameLogsRepository;

    public GetGameLogsHandler(IGameLogsRepository gameLogsRepository)
    {
        _gameLogsRepository = gameLogsRepository;
    }

    public async ValueTask<GetGameLogsResponse> Handle(GetGameLogsRequest request, CancellationToken cancellationToken)
    {
        List<GameLogDto> log = (await _gameLogsRepository.GetAllForPlayer(request.PlayerId))
            .Select(MapGameLogDto)
            .ToList();

        GetGameLogsResponse getGameLogsResponse = new GetGameLogsResponse
        {
            Logs = log
        };

        return getGameLogsResponse;
    }

    private GameLogDto MapGameLogDto(GameLog log)
    {
        GameLogDto gameLogDto = new GameLogDto()
        {
            Id = log.Id,
            Message = log.Message
        };

        return gameLogDto;
    }
}