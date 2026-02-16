using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Entity.Orders;
using AetherFire23.ERP.Domain.Entity.PurchaseOrderring;
using ERP.Integration;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace ERP.Testing.Domain.Entity.PurchaseOrderring;

[TestSubject(typeof(PurchaseOrder))]
public class PurchaseOrderTest : ErpIntegrationTestBase
{
    public PurchaseOrderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenPurchaseOrder_WhenAdded_ThenExistsInContext()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");
        Context.CompanyInfo.Add(companyInfo);
        var product = new Product
        {
            BasePrice = 12,
            ProductName = "Coke a Tintin",
        };
        Context.Products.Add(product);
        // The order is made...
        var order = new Order();
        var productLine = new OrderProductLine
        {
            QuantityOrdered = 12,
            ProductId = product.Id
        };
        order.OrderProductLines.Add(productLine);
        Context.Orders.Add(order);
        await Context.SaveChangesAsync();

        var purchaseOrder = new PurchaseOrder();
        Context.Add(purchaseOrder);
        Context.SaveChanges();

        // Assert.Equal(order.Id, productLine.OrderId);
        Assert.Contains(purchaseOrder, base.Context.PurchaseOrders);
    }

    [Fact]
    public async Task GivenPurchaseOrderProductLine_WhenAdded_ThenExistsInPurchaseOrder()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");
        Context.CompanyInfo.Add(companyInfo);
        var product = new Product
        {
            BasePrice = 12,
            ProductName = "Coke a Tintin",
        };
        Context.Products.Add(product);
        // The order is made...
        var order = new Order();
        var productLine = new OrderProductLine
        {
            QuantityOrdered = 12,
            ProductId = product.Id
        };
        order.OrderProductLines.Add(productLine);
        Context.Orders.Add(order);
        await Context.SaveChangesAsync();
        var purchaseOrder = new PurchaseOrder();
        Context.Add(purchaseOrder);
        await Context.SaveChangesAsync();
        // Rule of thumb : If a parent was persisted ( savechanges)
        // Before a child oculd be added, the chhild needs to be tracked before being added.


        const int MISSING_COUNT = 4;
        purchaseOrder.AddProductLine(product, MISSING_COUNT);
        foreach (PurchaseOrderProductLine purchaseOrderPurchaseOrderProductLine in purchaseOrder
                     .PurchaseOrderProductLines)
        {
            Context.Add(purchaseOrderPurchaseOrderProductLine);
        }

        await Context.SaveChangesAsync();


        // Assert.Equal(order.Id, productLine.OrderId);
        Assert.True(purchaseOrder.PurchaseOrderProductLines.Count == 1);
    }

    [Fact]
    public async Task GivenOrderItemReservation_WhenAdded_ThenExistsInProductLine()
    {
        var companyInfo = CompanyInfo.Create("FredCo inc. ltee.");
        Context.CompanyInfo.Add(companyInfo);
        var product = new Product
        {
            BasePrice = 12,
            ProductName = "Coke a Tintin",
        };
        Context.Products.Add(product);
        // The order is made...
        var order = new Order();
        var productLine = new OrderProductLine
        {
            QuantityOrdered = 12,
            ProductId = product.Id
        };
        order.OrderProductLines.Add(productLine);
        Context.Orders.Add(order);
        await Context.SaveChangesAsync();
        var purchaseOrder = new PurchaseOrder();
        Context.Add(purchaseOrder);
        const int MISSING_COUNT = 4;
        purchaseOrder.AddProductLine(product, MISSING_COUNT);

        purchaseOrder.AddOrderItemReservation(purchaseOrder.PurchaseOrderProductLines.First(), order, 4);
        await Context.SaveChangesAsync();

        Assert.True(purchaseOrder.PurchaseOrderProductLines.First().OrderItemReservations.Count == 1);
    }
}