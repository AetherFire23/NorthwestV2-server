using AetherFire23.Commons.Domain.Entities;
using AetherFire23.ERP.Domain.Entity.Orders;

namespace AetherFire23.ERP.Domain.Entity.PurchaseOrderring;

/// <summary>
/// Reserves a certain quantity 
/// </summary>
public class OrderItemReservation : EntityBase
{
    public Guid PurchaseOrderProductLineId { get; set; }
    public required PurchaseOrderProductLine PurchaseOrderProductLine { get; set; }

    public Guid OrderId { get; set; }
    public required Order Order { get; set; }

    public required int Quantity { get; set; }

    internal OrderItemReservation()
    {
    }
}