using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Entity.Orders;
using AetherFire23.ERP.Domain.Entity.PurchaseOrderring;
using Microsoft.EntityFrameworkCore;

namespace NorthwestV2.Practical;

public class ErpContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductItem> ProductItems { get; set; }
    public DbSet<CompanyInfo> CompanyInfo { get; set; }

    // Purchase Orders 
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderProductLine> PurchaseOrderProductLines { get; set; }
    public DbSet<OrderItemReservation> PurchaseOrderProductLineReserveForOrders { get; set; }

    // Orders 
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProductLine> OrderProductLines { get; set; }


    public ErpContext(DbContextOptions<ErpContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}