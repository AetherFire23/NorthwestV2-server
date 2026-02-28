using AetherFire23.ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace NorthwestV2.Practical;

public class NorthwestContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }

    public NorthwestContext(DbContextOptions<NorthwestContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}