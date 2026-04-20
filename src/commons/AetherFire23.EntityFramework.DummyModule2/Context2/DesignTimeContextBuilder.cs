using AetherFire23.Commons.EntityFramework.DummyConsole.Context2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AertherFire23.EntityFramework.DummyModule2.Context2;

public class DesignTimeContextBuilder : IDesignTimeDbContextFactory<MyContext2>
{
    public MyContext2 CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MyContext2>()
            .UseNpgsql("Host=localhost;Database=FakeDesignTime;Username=fake;Password=fake;")
            .Options;

        return new MyContext2(options);
    }
}