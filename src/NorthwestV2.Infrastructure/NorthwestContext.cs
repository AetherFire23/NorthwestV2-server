using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application;

namespace NorthwestV2.Infrastructure;

/*
 * I did the whole UOW patteern initially for mocking; separartion. It's not necessary.
 * However I like having my queries elsewhere.
 * https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking?redirectedfrom=MSDN
 *
 */
public class NorthwestContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<ItemBase> Items { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    public NorthwestContext(DbContextOptions<NorthwestContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemBase>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<ItemBase>(nameof(ItemBase))
            .HasValue<NormalItemBase>("NormalItemBase")
            .HasValue<ProductionItemBase>("ProductionItemBase")
            .HasValue<UnfinishedSpyglass>("UnfinishedSpyglass");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // calls himself recursively if you dont do base. 
        var changes = await base.SaveChangesAsync();

        return changes;
    }
}