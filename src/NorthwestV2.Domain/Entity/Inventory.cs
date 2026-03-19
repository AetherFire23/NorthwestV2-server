namespace AetherFire23.ERP.Domain.Entity;

/// <summary>
/// BOth a player and a Room can be an Item owner. Introducing the ivnentory makes things so much easier 
/// </summary>
public class Inventory : EntityBase
{
    public Guid? PlayerId { get; set; }
    public Player? Player { get; set; }

    public Guid? RoomId { get; set; }
    public Room? Room { get; set; }
    public List<Item> Items { get; set; } = [];
}