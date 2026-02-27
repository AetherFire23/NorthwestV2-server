using AetherFire23.Commons.Composition;
using AetherFire23.Commons.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace AetherFire23.Commons.EntityFramework.NpgsqlHelper;

/// <summary>
/// Base class for initializing a npgsql database. 
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public abstract class NpgsqlInitializable<TDbContext> : IInitializer, IDatabaseNameProvider
    where TDbContext : DbContext
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Get the connection string from the derived class. 
        serviceCollection.AddDbContext<TDbContext>(action =>
        {
            // Use the derived class's definition of ConfigureAddDbContext
            action.UseNpgsql(GetConnectionStringInConfiguration(configuration), ConfigureAddDbContext);
        });
    }

    public virtual void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        using var db = scope.ServiceProvider.GetRequiredService<TDbContext>();

        // No need to do EnsureDeleted because it will always be a clean-slate test container by default.
        // Method is virtual, override as needed. 
        db.Database.Migrate();
    }

    /// <summary>
    /// Provide additional configuration
    /// </summary>
    /// <param name="serviceCollection"></param>
    protected virtual void ConfigureAddDbContext(NpgsqlDbContextOptionsBuilder serviceCollection)
    {
    }

    /// <summary>
    /// convention : databaseName + Key
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private string GetConnectionStringInConfiguration(IConfiguration configuration)
    {
        string connectionStringKey = $"{ProvideDatabaseName()}ConnectionString";

        var connectionString = configuration.GetSection(connectionStringKey).Value
                               ?? throw new Exception(
                                   $"Connection string with key {connectionStringKey} not found in Configuration");

        return connectionString;
    }

    /// <summary>
    ///  create schema by convention from the context's name. contained in the closed generic type parameter of the basetype. 
    /// </summary>
    public string ProvideDatabaseName()
    {
        string databaseName = this.GetType()
            .GetBaseTypeOrThrow()
            .GetGenericArguments()
            .First().Name.Replace("Context", string.Empty)
            .ToLower();

        return databaseName;
    }
}