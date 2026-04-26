using NorthwestV2.Features.UseCases.GameLogs;

namespace NorthwestV2.Features.Features.Shared.Entity;

public class GameLog : EntityBase
{
    public Guid PlayerId { get; set; }
    public required Player Player { get; set; }

    public required string Message { get; set; }
}