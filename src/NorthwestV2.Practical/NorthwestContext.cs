using AetherFire23.ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application;

namespace NorthwestV2.Practical;

public class NorthwestContext : DbContext
{
    private readonly RequestContextService _requestContextService;
    public DbSet<User> Users { get; set; }

    public NorthwestContext(DbContextOptions<NorthwestContext> options, RequestContextService requestContextService) : base(options)
    {
        _requestContextService = requestContextService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}