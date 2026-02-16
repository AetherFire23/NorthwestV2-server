using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Entity.Orders;
using ERP.Integration;
using Xunit.Abstractions;

namespace ERP.Testing.Domain;

public class UnitTest1 : ErpIntegrationTestBase
{
    public UnitTest1(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Test1()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");

        base.Context.CompanyInfo.Add(companyInfo);

        var product = new Product
        {
            BasePrice = 12,
            ProductName = "Coke a Tintin",
        };

        var product2 = new Product()
        {
            BasePrice = 44,
            ProductName = "BloopProduct",
        };

        base.Context.Products.Add(product);
        base.Context.Products.Add(product2);

        var orderEntity = base.Context.Orders.Add(new Order());

        var productLine = new OrderProductLine
        {
            ProductId = product.Id,
            QuantityOrdered = 12,
        };

        base.Context.Add(productLine);


        orderEntity.Entity.OrderProductLines.Add(productLine);

        await base.Context.SaveChangesAsync();


        Assert.Contains(orderEntity.Entity, Context.Orders);
    }
}