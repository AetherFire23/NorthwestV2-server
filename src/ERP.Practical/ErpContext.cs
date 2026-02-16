using AetherFire23.ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace NorthwestV2.Practical;

public class ErpContext : DbContext
{
    public DbSet<User> Users { get; set; }



    public ErpContext(DbContextOptions<ErpContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}