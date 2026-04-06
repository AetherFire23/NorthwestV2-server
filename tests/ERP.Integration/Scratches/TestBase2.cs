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

public class TestBase2 : IAsyncLifetime
{
    private readonly ITestOutputHelper _output;

    protected IServiceProvider RootServiceProvider;
    protected IServiceScope Scope;

    protected IMediator Mediator => Scope.ServiceProvider.GetRequiredService<IMediator>();
    protected NorthwestContext Context => Scope.ServiceProvider.GetRequiredService<NorthwestContext>();

    private string _currentDbName;

    protected TestBase2(ITestOutputHelper output)
    {
        _output = output;
    }

    public async Task InitializeAsync()
    {
        //  Get shared container
        PostgreSqlContainer container = await SharedContainerFixture.GetAsync();

        //  Create a fresh DB for this test
        _currentDbName = $"nw_{Guid.NewGuid():N}";
        await CreateDatabaseAsync(container, _currentDbName);

        // Replaces the default connection string given by the container to a custom uniquely-generated one. 
        string connectionString = new NpgsqlConnectionStringBuilder(container.GetConnectionString())
        {
            Database = _currentDbName
        }.ToString();

        // 3. Build DI
        ServiceCollection services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddProvider(new XUnitLoggerProvider(_output));
            builder.SetMinimumLevel(LogLevel.Information);
        });

        // Added the uniquely-generated connection string to the database. 
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["northwestConnectionString"] = connectionString
            })
            .Build();

        AppComposer.ComposeApplication(services, config);

        RootServiceProvider = services.BuildServiceProvider();
        AppComposer.Initialize(RootServiceProvider);

        Scope = RootServiceProvider.CreateScope();
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await DropDatabaseAsync();
    }

    private static async Task CreateDatabaseAsync(PostgreSqlContainer container, string dbName)
    {
        await using NpgsqlConnection conn = new NpgsqlConnection(container.GetConnectionString());
        await conn.OpenAsync();

        await using NpgsqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = $"CREATE DATABASE \"{dbName}\";";
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task DropDatabaseAsync()
    {
        PostgreSqlContainer container = await SharedContainerFixture.GetAsync();
        var dbName = _currentDbName; // store this when you create it

        await using NpgsqlConnection conn = new NpgsqlConnection(container.GetConnectionString());
        await conn.OpenAsync();
        
        

        // Force disconnect all sessions except the one killing the sessions
        await using (NpgsqlCommand terminate = conn.CreateCommand())
        {
            terminate.CommandText = $@"
            SELECT pg_terminate_backend(pid)
            FROM pg_stat_activity
            WHERE datname = '{dbName}' AND pid <> pg_backend_pid();";
            await terminate.ExecuteNonQueryAsync();
        }

        // Drop the DB
        await using NpgsqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = $"DROP DATABASE IF EXISTS \"{dbName}\";";
        await cmd.ExecuteNonQueryAsync();
    }

    protected T GetServiceFromScope<T>() where T : notnull
    {
        T service = this.Scope.ServiceProvider.GetRequiredService<T>();

        return service;
    }
}