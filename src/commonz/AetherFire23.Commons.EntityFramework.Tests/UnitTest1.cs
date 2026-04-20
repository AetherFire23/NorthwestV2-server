using AetherFire23.Commons.Composition;
using AetherFire23.Commons.EntityFramework.DummyConsole.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Requests;
using Testcontainers.PostgreSql;

namespace AetherFire23.Commons.EntityFramework.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        // Creer un container
        var postgreSqlContainer = new PostgreSqlBuilder("postgres:18").WithDatabase("allo").Build();

        await postgreSqlContainer.StartAsync();

        // creer une configuration avec ca. 

        var dic = new Dictionary<string, string>
            { { "myConnectionString", postgreSqlContainer.GetConnectionString() } };

        var config = new ConfigurationBuilder().AddInMemoryCollection(dic).Build();
        var sc = new ServiceCollection();
        var composer = new Composer();

        composer.InstallServices(sc, config, typeof(MyContext).Assembly);

        var sp = sc.BuildServiceProvider();

        composer.InitializeServices(sp);
    }
}