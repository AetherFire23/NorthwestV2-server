using System.Reflection;
using System.Text.RegularExpressions;
using AetherFire23.Commons.Composition;
using AetherFire23.Commons.EntityFramework.NpgsqlHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

// ReSharper disable LoopCanBeConvertedToQuery

/*
 ReSharper disable VirtualMemberCallInConstructor
 This warning is disabled because it's the intended way to initialize the base class. The derived class's methods are pure.
*/

namespace AetherFire23.Commons.Testing;

public abstract class PostgresTestContainer
{
    // Make an IInitializable that accepts. 
    // At worst, I could abstract the container and check if we can split by schema instead. 
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IServiceCollection _serviceCollection;
    protected ITestOutputHelper Output;

    protected PostgresTestContainer(ITestOutputHelper outputHelper)
    {
        Output = outputHelper;
        // Launch the postgres container
        var instance =
            new PostgreSqlBuilder("postgres:18").Build();

        instance.StartAsync().Wait();

        /*
         TestContainers provides only 1 connection string it a fixed database name.
         To split/define different schemas, we're replacing the database name at configuration-time
         This is why we are settiung configurations strings
        */
        var kvps = SetDatabaseConfigurationStrings(instance.GetConnectionString());

        var providedConfigurationEntries = ProvideAdditionalConfigurationEntries();

        var allConfigurations = kvps.Union(providedConfigurationEntries);

        Dictionary<string, string?> configurationDictionary = new(allConfigurations);

        IConfigurationRoot configuration =
            new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();

        // Install services with composers

        ServiceCollection serviceCollection = new();

        Composer composer = new();

        // Casting to array for params[] compatibility
        composer.InstallServices(serviceCollection, configuration,
            ProvideInstallerAssemblies().ToArray());


        this.ConfigureAdditionalServices(serviceCollection);

        _serviceCollection = serviceCollection;

        _serviceProvider = serviceCollection.BuildServiceProvider();

        composer.InitializeServices(_serviceProvider);
    }

    /// <summary>
    /// Add more configurations i.e. connection strings. 
    /// in api its already configured. In test, config needs to be mocked. 
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerable<KeyValuePair<string, string?>> ProvideAdditionalConfigurationEntries()
    {
        return [];
    }

    /// <summary>
    /// Provide the test assemblies and the TEST / seeding assemblies that need to be registered
    /// Also this method will be called by the parent's constructor so do not reference anything inside the current
    /// constructor. (you shouldn't).
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    protected abstract IEnumerable<Assembly> ProvideInstallerAssemblies();

    /// <summary>
    /// COnfgirues additional services. Runs after installing IINStallers therefore will override same-tyhpe registrations
    /// </summary>
    /// <param name="serviceCollection"></param>
    protected virtual void ConfigureAdditionalServices(IServiceCollection serviceCollection)
    {
    }

    // TODO: if needed 
    private void SwapInstallers()
    {
    }

    protected TService GetService<TService>() where TService : class
    {
        return _serviceProvider.GetRequiredService<TService>();
    }

    /// <summary>
    /// Sets the connection strings for each initializer of database found. 
    /// </summary>
    private IEnumerable<KeyValuePair<string, string?>> SetDatabaseConfigurationStrings(
        string baseConnectionString)
    {
        // get all database installers in all assemblies
        // Then instantiante them to get a database name 
        var databaseNames = ProvideInstallerAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(t => typeof(IDatabaseNameProvider).IsAssignableFrom(t))
            .Select(t =>
                Activator.CreateInstance(t) as IDatabaseNameProvider ??
                throw new InvalidOperationException($"Casting error to {nameof(IDatabaseNameProvider)} failed"))
            .Select(x => x.ProvideDatabaseName())
            .ToList();

        // baseConnectionString.Replace("Database=postgres", $"Database={databaseInstallers.First()}")

        IEnumerable<KeyValuePair<string, string>> kvps = databaseNames
            .Select(x =>
            {
                string key = x + "ConnectionString";
                string value = Regex.Replace(baseConnectionString, "Database=[a-z0-9]*;", $"Database={x};");
                return KeyValuePair.Create(key, value);
            });

        return kvps;
    }
}