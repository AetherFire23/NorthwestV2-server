namespace AetherFire23.ERP.Domain.Entity;

public class Game : EntityBase
{
    public List<Player> Players { get; set; } = [];
    public List<Room> Rooms { get; set; } = [];
}