using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class Warehouse : EntityBase
{
    public required string WarehouseName { get; set; }

    public required Guid CompanyId { get; set; }

    public CompanyInfo? Company { get; set; }

    public virtual ICollection<ProductItem> ProductItems { get; set; } = [];

    public ProductItem AddProductItem(Product product, int quantity)
    {
        var productItem = new ProductItem
        {
            Product = product,
            Quantity = quantity,
            Warehouse = this,
        };

        this.ProductItems.Add(productItem);

        return productItem;
    }
}