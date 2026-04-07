namespace NorthwestV2.Features.Features.Shared.Entity;

public class Game : EntityBase
{
    public virtual List<Player> Players { get; set; } = [];
    public virtual List<Room> Rooms { get; set; } = [];
}