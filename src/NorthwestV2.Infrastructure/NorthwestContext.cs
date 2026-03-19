using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
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
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Production> Productions { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    

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

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // calls himself recursively if you dont do base. 
        var changes = await base.SaveChangesAsync(ct);

        int i = 0;

        return changes;
    }
}

public class Blog
{
    public Guid Id { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>();
}

public class Post
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public Blog Blog { get; set; }
}