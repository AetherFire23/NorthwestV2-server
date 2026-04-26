namespace NorthwestV2.Features.Features.Shared.Entity;

public class GameLog : EntityBase
{
    public virtual ICollection<Player> Players { get; set; }
    public required string Message { get; set; }
}