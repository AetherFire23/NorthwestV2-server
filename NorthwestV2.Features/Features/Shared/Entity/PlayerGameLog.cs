namespace NorthwestV2.Features.Features.Shared.Entity;

public class PlayerGameLog : EntityBase
{
    public Guid PlayerId { get; set; }
    public virtual required Player Player { get; set; }

    public Guid GameLogId { get; set; }
    public virtual required GameLog GameLog { get; set; }
}