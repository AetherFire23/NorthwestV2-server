namespace NorthwestV2.Features.Features.Shared.Entity;

public class GameLog : EntityBase
{
    public virtual ICollection<PlayerGameLog> PlayerGameLogs { get; set; } = [];
    public required string Message { get; set; }
}