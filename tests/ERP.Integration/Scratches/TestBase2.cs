using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthwestV2.Compose;
using NorthwestV2.Infrastructure;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Scratches;

public class TestBase2: IAsyncLifetime
{
    private readonly ITestOutputHelper _output;

    protected IServiceProvider RootServiceProvider = default!;
    protected IServiceScope Scope = default!;

    protected IMediator Mediator => Scope.ServiceProvider.GetRequiredService<IMediator>();
    protected NorthwestContext Context => Scope.ServiceProvider.GetRequiredService<NorthwestContext>();

    protected TestBase2(ITestOutputHelper output)
    {
        _output = output;
    }

    public async Task InitializeAsync()
    {
        // 1. Get shared container
        var container = await SharedContainerFixture.GetAsync();

        // 2. Create a fresh DB for THIS test
        var dbName = $"nw_{Guid.NewGuid():N}";
        await CreateDatabaseAsync(container, dbName);

        var connectionString = new NpgsqlConnectionStringBuilder(container.GetConnectionString())
        {
            Database = dbName
        }.ToString();

        // 3. Build DI
        var services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddProvider(new XUnitLoggerProvider(_output));
            builder.SetMinimumLevel(LogLevel.Information);
        });

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["northwestConnectionString"] = connectionString
            })
            .Build();

        AppComposer.ComposeApplication(services, config);

        RootServiceProvider = services.BuildServiceProvider();
        AppComposer.Initialize(RootServiceProvider);

        // 4. Create scope
        Scope = RootServiceProvider.CreateScope();
    }

    public Task DisposeAsync()
    {
        Scope.Dispose();
        return Task.CompletedTask;
    }

    private static async Task CreateDatabaseAsync(PostgreSqlContainer container, string dbName)
    {
        await using var conn = new NpgsqlConnection(container.GetConnectionString());
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"CREATE DATABASE \"{dbName}\";";
        await cmd.ExecuteNonQueryAsync();
    }
}