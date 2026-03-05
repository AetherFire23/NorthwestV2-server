using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

/// <summary>
/// BOth a player and a Room can be an Item owner. Introducing the ivnentory makes things so much easier 
/// </summary>
public class Inventory : EntityBase
{
    public List<Item> Items { get; set; } = [];
}