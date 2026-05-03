namespace NorthwestV2.Features.Features.Shared.Entity;

public class GameLog : EntityBase
{
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public required string Message { get; set; }
}