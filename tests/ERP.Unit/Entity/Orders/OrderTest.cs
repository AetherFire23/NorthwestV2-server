using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Entity.Orders;
using ERP.Integration;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace ERP.Testing.Domain.Entity.Orders;

[TestSubject(typeof(Order))]
public class OrderTest : ErpIntegrationTestBase
{
    public OrderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenAProductLine_WhenAddedDirectlyToCollection_ThenOrderIdSetAutomatically()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");
        Context.CompanyInfo.Add(companyInfo);

        var product = new Product
        {
            BasePrice = 12,
            ProductName = "Coke a Tintin",
        };

        Context.Products.Add(product);

        var order = new Order();

        var productLine = new OrderProductLine
        {
            QuantityOrdered = 12,
            ProductId = product.Id
        };

        order.OrderProductLines.Add(productLine);
        Context.Orders.Add(order);

        await Context.SaveChangesAsync();

        Assert.Equal(order.Id, productLine.OrderId);
    }

    [Fact]
    public async Task GivenAProductLine_WhenAddedDirectlyToCollection_ThenOrderIdSetAutomatically2()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");
        Context.CompanyInfo.Add(companyInfo);

        var product = new Product
        {
            BasePrice = 12,
            ProductName = "Coke a Tintin",
        };

        Context.Products.Add(product);

        // Create the new Order
        var order = new Order();
        Context.Orders.Add(order);

        // Create a product Line 
        var productLine = new OrderProductLine
        {
            QuantityOrdered = 12,
            ProductId = product.Id
        };

        order.OrderProductLines.Add(productLine);
        await Context.SaveChangesAsync();


        await Context.SaveChangesAsync();

        Assert.Equal(order.Id, productLine.OrderId);
    }

    [Fact]
    public async Task GivenAProductLine_WhenAddedDirectlyToCollection_ThenOrderIdSetAutomatically3()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");
        base.Context.CompanyInfo.Add(companyInfo);
        var product = new Product { BasePrice = 12, ProductName = "Coke a Tintin", };
        var product2 = new Product() { BasePrice = 44, ProductName = "BloopProduct", };
        base.Context.Products.Add(product);
        base.Context.Products.Add(product2);
        base.Context.Products.Add(product2);
        base.Context.Products.Add(product2);
        base.Context.Products.Add(product2);
        base.Context.Products.Add(product2);
        base.Context.Products.Add(product2);
        var order = new Order();
        Context.Orders.Add(order);
        await base.Context.SaveChangesAsync();

        order = Context.Orders.First(x => x.Id == order.Id);

        var productLine = new OrderProductLine
        {
            ProductId = product.Id,
            QuantityOrdered = 12,
        };

        Context.Add(productLine);
        order.OrderProductLines.Add(productLine);

        await base.Context.SaveChangesAsync();
    }

    [Fact]
    public async Task GivenAProductLine_WhenAddedViaAddProductMethod_ThenOrderIsProductAdded()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");
        base.Context.CompanyInfo.Add(companyInfo);
        var product = new Product { BasePrice = 12, ProductName = "Coke a Tintin", };
        var product2 = new Product() { BasePrice = 44, ProductName = "BloopProduct", };
        base.Context.Products.Add(product);
        await base.Context.SaveChangesAsync();

        Context.Add(product2);

        base.Context.Products.Add(product2);

        var order = new Order();
        Context.Add(order);

        order.AddProductLine(product, 123);

        await base.Context.SaveChangesAsync();
    }
};