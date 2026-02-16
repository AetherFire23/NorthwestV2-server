using AetherFire23.Commons.Domain.Entities;
using AetherFire23.ERP.Domain.Entity.Orders;

namespace AetherFire23.ERP.Domain.Entity.PurchaseOrderring;

public class PurchaseOrder : EntityBase
{
    public ICollection<PurchaseOrderProductLine> PurchaseOrderProductLines { get; set; } = [];

    public void AddProductLine(Product product, int quantity)
    {
        PurchaseOrderProductLine purchaseOrderProductLine = new PurchaseOrderProductLine()
        {
            QuantityOrdered = quantity,
            Product = product,
            PurchaseOrder = this,
        };

        PurchaseOrderProductLines.Add(purchaseOrderProductLine);
    }

    /// <summary>
    /// Reserves an item's quantity on a purchase Order 
    /// </summary>
    public void AddOrderItemReservation(PurchaseOrderProductLine purchaseOrderProductLine, Order order,
        int quantityReserved)
    {
        OrderItemReservation orderItemReservation = new OrderItemReservation
        {
            PurchaseOrderProductLine = purchaseOrderProductLine,
            Quantity = quantityReserved,
            Order = order
        };

        purchaseOrderProductLine.OrderItemReservations.Add(orderItemReservation);
    }
}