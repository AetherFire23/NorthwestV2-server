using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace NorthwestV2.Infrastructure.Contexts;

public static class NorthwestContextInstaller
{
    public static void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Get the connection string from the derived class. 
        serviceCollection.AddDbContext<NorthwestContext>(action =>
        {
            // Use the derived class's definition of ConfigureAddDbContext
            action.UseNpgsql(GetConnectionStringInConfiguration(configuration), c => { });
        });

        serviceCollection.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<NorthwestContext>());
    }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<NorthwestContext>();

        // No need to do EnsureDeleted because it will always be a clean-slate test container by default.
        // Method is virtual, override as needed. 
        db.Database.Migrate();
    }


    /// <summary>
    /// convention : databaseName + Key
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static string GetConnectionStringInConfiguration(IConfiguration configuration)
    {
        string connectionStringKey = $"northwestConnectionString";

        var connectionString = configuration.GetSection(connectionStringKey).Value
                               ?? throw new Exception(
                                   $"Connection string with key {connectionStringKey} not found in Configuration");

        return connectionString;
    }
}