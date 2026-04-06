namespace NorthwestV2.Features.Features.Shared.Entity;

public class Game : EntityBase
{
    public List<Player> Players { get; set; } = [];
    public List<Room> Rooms { get; set; } = [];
}