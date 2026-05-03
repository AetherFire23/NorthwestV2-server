using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features;
using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Actions.Productions.Core.Entities;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;

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
    public DbSet<GameLog> Logs { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<RoomConnection> RoomConnection { get; set; }

    public NorthwestContext(DbContextOptions<NorthwestContext> options) : base(options)
    {
    }

    // TODO: Do a configuration file for each entity 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemBase>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<ItemBase>(nameof(ItemBase))
            .HasValue<CommonItemBase>(nameof(CommonItemBase))
            .HasValue<ProductionItemBase>(nameof(ProductionItemBase))
            .HasValue<UnfinishedSpyglass>(nameof(UnfinishedSpyglass))
            .HasValue<UnfinishedFishingPole>(nameof(UnfinishedFishingPole))
            .HasValue<Hammer>(nameof(Hammer))
            .HasValue<Spyglass>(nameof(Spyglass))
            .HasValue<UnfinishedHammer>(nameof(UnfinishedHammer))
            .HasValue<Scrap>(nameof(Scrap));

        modelBuilder.Entity<ProductionItemBase>(x =>
        {
            x.Property(x => x.CurrentStageContribution)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<StageContributionBase>(v, (JsonSerializerOptions?)null)!
                );
            // .Metadata.SetValueComparer(new ValueComparer<StageBase>(
            //     (c1, c2) => JsonSerializer.Serialize(c1, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(c2, (JsonSerializerOptions?)null),
            //     c => c == null ? 0 : JsonSerializer.Serialize(c, (JsonSerializerOptions?)null).GetHashCode(),
            //     c => JsonSerializer.Deserialize<StageBase>(JsonSerializer.Serialize(c, (JsonSerializerOptions?)null), (JsonSerializerOptions?)null)!));
        });


        /*
         * Do the
         */
        // Composite PK

        modelBuilder.Entity<RoomConnection>(entity =>
        {
            // Follower side
            entity.HasOne(uf => uf.Room1)
                .WithMany(u => u.Connections)
                .HasForeignKey(uf => uf.Room1Id)
                .OnDelete(DeleteBehavior.Restrict); // avoid cascade cycles
            
            // Followed side
            entity.HasOne(uf => uf.Room2)
                .WithMany(u => u.Connections)
                .HasForeignKey(uf => uf.Room2Id)
                .OnDelete(DeleteBehavior.Restrict);
        });



;


        // TODO: Automatic discovery of children types of items 
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // calls himself recursively if you dont do base. 
        var changes = await base.SaveChangesAsync();

        return changes;
    }
}