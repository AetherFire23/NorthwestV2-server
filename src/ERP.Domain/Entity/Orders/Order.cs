using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity.Orders;

public class Order : EntityBase
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public ICollection<OrderProductLine> OrderProductLines { get; set; } = [];

    // TODO: IDEA: for analyzer
    // an entity marked with "entitybase" must be if created within a class that inherits for entitybase 
    public OrderProductLine AddProductLine(Product product, int quantity)
    {
        OrderProductLine orderProductLine = new OrderProductLine
        {
            Product = product,
            QuantityOrdered = quantity,
            Order = this,
        };

        this.OrderProductLines.Add(orderProductLine);

        return orderProductLine;
    }

    public bool HasLine(Product product)
    {
        return this.OrderProductLines.Any(x => x.Product.Equals(product));
    }

    public bool HasLine(Guid productId)
    {
        return this.OrderProductLines.Any(x => x.Product.Id.Equals(productId));
    }

    public OrderProductLine GetLine(Product product)
    {
        OrderProductLine prod = this.OrderProductLines.First(x => x.Product.Id == product.Id);
        return prod;
    }

    public OrderProductLine GetLine(Guid productId)
    {
        OrderProductLine prod = this.OrderProductLines.First(x => x.ProductId == productId);
        return prod;
    }
}