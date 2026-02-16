using AetherFire23.Commons.Composition.DummyProject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.Commons.Composition.tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var sc = new ServiceCollection();

        Dictionary<string, string> dic = new Dictionary<string, string>();
        var composer = new Composer();

        composer.InstallServices(sc, new ConfigurationBuilder().AddInMemoryCollection(dic).Build(),
            typeof(InstalledAndInitializedClass).Assembly);

        var sp = sc.BuildServiceProvider();


        var service = sp.GetRequiredService<InstalledAndInitializedClass>();
    }

    [Fact]
    public void Test2()
    {
        var sc = new ServiceCollection();

        var composer = new Composer();

        composer.InstallServices(sc,
            new ConfigurationBuilder().Build(),
            typeof(InstalledAndInitializedClass).Assembly);


        // sc.RemoveInstaller<InstalledAndUninstallezInitializer>();

        var sp = sc.BuildServiceProvider();

        // sp.InitializeServices();
    }

    [Fact]
    public void Test3()
    {
        bool res = typeof(IInstaller).FullName == typeof(IInstaller).FullName;

        bool res2 = typeof(IInstaller) == typeof(IInstaller);
    }

    [Fact]
    public void Test4()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();
    }
}