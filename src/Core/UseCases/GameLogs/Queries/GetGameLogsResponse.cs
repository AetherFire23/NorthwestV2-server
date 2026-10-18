namespace NorthwestV2.Features.UseCases.GameLogs.Queries;

public class GetGameLogsResponse
{
    public List<GameLogDto> Logs { get; set; } = [];
}