using AetherFire23.Commons.EntityFramework.DummyConsole.Context2;
using AetherFire23.Commons.EntityFramework.DummyConsole.Db;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace AetherFire23.Commons.Testing.DummyTestingProject;

public class UnitTest1 : BaseDummyTestingProjectClass
{
    public UnitTest1(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void Test1()
    {
        var s = base.GetService<MyContext>();

        s.MyOtherEntities.Add(new()
        {
            Id = Guid.NewGuid(),
        });

        s.SaveChanges();

        var s2 = base.GetService<MyContext2>();

        s2.MyUsers.Add(new()
        {
            Id = Guid.NewGuid(),
        });

        s2.SaveChanges();
    }

    [Fact]
    public void Test3()
    {
    }

    [Fact]
    public void Test4()
    {
        var sc = new ServiceCollection();

        sc.AddKeyedScoped<MyBoy>(Keys.Allo);


        var sp = sc.BuildServiceProvider();

        var allo = sp.GetRequiredKeyedService<MyBoy>(Keys.Allo);
    }
}

public abstract class BoyBase
{
    public abstract string Greet();
}

public class MyBoy : BoyBase
{
    public override string Greet()
    {
        return "bloop";
    }
}

public enum Keys
{
    Allo,
    Bello,
}