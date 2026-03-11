using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NorthwestV2.Infrastructure;

namespace NorthwestV2.Practical;

// TODO: check if Commons can handle a generic implementation 
public class MigrationsFactory : IDesignTimeDbContextFactory<NorthwestContext>
{
    public NorthwestContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<NorthwestContext>()
            .UseNpgsql("Host=localhost;Database=FakeDesignTime;Username=fake;Password=fake;")
            .Options;

        return new NorthwestContext(options);
    }
}