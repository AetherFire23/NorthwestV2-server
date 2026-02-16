using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity.Orders;

public class OrderProductLine : EntityBase
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public required int QuantityOrdered { get; set; }
}