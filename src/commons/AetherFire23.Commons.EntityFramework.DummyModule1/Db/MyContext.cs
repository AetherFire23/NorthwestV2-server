using AetherFire23.Commons.EntityFramework.DummyConsole.MyEntities2;
using Microsoft.EntityFrameworkCore;

namespace AetherFire23.Commons.EntityFramework.DummyConsole.Db;

public class MyContext : DbContext
{
    public DbSet<MyOtherEntity> MyOtherEntities { get; set; }

    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }
}