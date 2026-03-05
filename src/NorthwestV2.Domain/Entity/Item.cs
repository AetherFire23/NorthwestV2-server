using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class Item : EntityBase
{
    public required ItemTypes ItemType { get; set; }
}