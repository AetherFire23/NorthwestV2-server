using AetherFire23.Commons.EntityFramework.DummyConsole.MyEntities;
using Microsoft.EntityFrameworkCore;

namespace AetherFire23.Commons.EntityFramework.DummyConsole.Context2;

public class MyContext2 : DbContext
{
    public DbSet<MyUser> MyUsers { get; set; }

    public MyContext2(DbContextOptions<MyContext2> options) : base(options)
    {
    }
}