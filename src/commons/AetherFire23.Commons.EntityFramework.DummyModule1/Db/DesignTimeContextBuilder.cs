using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace AetherFire23.Commons.EntityFramework.DummyConsole.Db;

public class DesignTimeContextBuilder : IDesignTimeDbContextFactory<MyContext>
{
    public MyContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseNpgsql("Host=localhost;Database=FakeDesignTime;Username=fake;Password=fake;") 
            .Options;
        
        return new MyContext(options);
    }
}