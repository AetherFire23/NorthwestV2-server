using AetherFire23.ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application;

namespace NorthwestV2.Infrastructure;

public class NorthwestContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Log> Logs { get; set; }

    public NorthwestContext(DbContextOptions<NorthwestContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Item>()
        //     .HasDiscriminator(i => i.ItemType)
        //     .HasValue<Item>(ItemTypes.);
    }
    //
    // var productionItems = await _db.Items
    //     .OfType<ProductionItem>()
    //     .ToListAsync();

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        var changes = await base.SaveChangesAsync(ct);

        int i = 0;
    }
}