namespace NorthwestV2.Features.UseCases.GameLogs;

public class GameLogDto
{
    public required Guid Id { get; init; }
    public required string Message { get; init; }
}