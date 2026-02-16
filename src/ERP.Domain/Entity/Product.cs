using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class Product : EntityBase
{
    public required string ProductName { get; set; }
    public required decimal BasePrice { get; set; }
}