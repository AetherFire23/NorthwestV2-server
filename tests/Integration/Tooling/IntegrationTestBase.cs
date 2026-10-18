using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Tooling;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    private string _currentDbName;
    IConfigurationRoot configuration;
    protected IServiceScope CurrentScope;
    public ServiceProvider RootServiceProvider { get; set; }
    private readonly ITestOutputHelper _output;

    public IntegrationTestBase(ITestOutputHelper output)
    {
        _output = output;
    }

    public virtual async Task InitializeAsync()
    {
        // Shared containers !
        PostgreSqlContainer container = await SharedContainerFixture.GetAsync();

        // Creates a unique database name because it is going to live alongisde all the other databases in the same container.
        // This is to avoid conflicts with other tests running in parallel.
        _currentDbName =
            $"nw_{Guid.NewGuid():N}"; // String Db name this when I create it so it can be disposed of later.   
        await CreateDatabaseAsync(container, _currentDbName);

        ServiceCollection serviceCollection = new ServiceCollection();

        Dictionary<string, string> configurations = new Dictionary<string, string>();

        string finalConnectionString = new NpgsqlConnectionStringBuilder(container.GetConnectionString())
        {
            Database = _currentDbName
        }.ToString();

        configurations.Add($"ConnectionStrings:{ProvideDatabaseName()}", finalConnectionString);

        await AddRequiredConfigurations(configurations);

        serviceCollection.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddProvider(new XUnitLoggerProvider(_output));
            builder.SetMinimumLevel(LogLevel.Information);
        });

        configuration = new ConfigurationBuilder().AddInMemoryCollection(configurations).Build();

        await AddServices(serviceCollection, configuration);

        RootServiceProvider = serviceCollection.BuildServiceProvider();

        await InitializeServices(RootServiceProvider, configuration);

        CurrentScope = RootServiceProvider.CreateScope();
    }

    /// <summary>
    /// Watch out, name must match the actual expected connection string key ! 
    /// </summary>
    /// <returns></returns>
    public abstract string ProvideDatabaseName();

    public abstract Task AddRequiredConfigurations(Dictionary<string, string> configurations);

    public abstract Task AddServices(IServiceCollection serviceCollection, IConfiguration configuration);

    public abstract Task InitializeServices(IServiceProvider serviceProvider, IConfiguration configuration);

    public async Task DisposeAsync()
    {
        CurrentScope.Dispose();
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
        string dbName = _currentDbName; // store this when you create it

        await using NpgsqlConnection conn = new NpgsqlConnection(container.GetConnectionString());
        await conn.OpenAsync();


        // Force disconnect all sessions except the one killing the sessions
        // not an AI-specific query.
        // It's an old, frequent boilerplate found everywhere. 
        // 15yo SO thread with almost the same query https://stackoverflow.com/questions/5108876/kill-a-postgresql-session-connection
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

    protected TService GetServiceFromScope<TService>()
        where TService : notnull
    {
        TService services = CurrentScope.ServiceProvider.GetRequiredService<TService>();

        return services;
    }

    /// <summary>
    /// Doesnt refresh the services that mgiht have been associated with a scope. 
    /// </summary>
    protected void RefreshScope()
    {
        CurrentScope = RootServiceProvider.CreateScope();
    }
}