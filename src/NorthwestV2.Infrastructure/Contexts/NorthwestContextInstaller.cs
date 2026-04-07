using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features;

namespace NorthwestV2.Infrastructure.Contexts;

public static class NorthwestContextInstaller
{
    public static void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Get the connection string from the derived class. 
        serviceCollection.AddDbContext<NorthwestContext>(action =>
        {
            // Use the derived class's definition of ConfigureAddDbContext
            action.UseNpgsql(GetConnectionStringInConfiguration(configuration), c =>
            {
                
            }).UseLazyLoadingProxies();
        });

        serviceCollection.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<NorthwestContext>());
    }

    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        /*
         * DisposeAsync vs normal dispoe
         * proprely handles IO when reaching end of scope for stuff like seeding
         * all-the-way-clean
         */
        using IServiceScope scope = serviceProvider.CreateAsyncScope();

        NorthwestContext db = scope.ServiceProvider.GetRequiredService<NorthwestContext>();

        // No need to do EnsureDeleted because it will always be a clean-slate test container by default.
        // Method is virtual, override as needed. 
        await db.Database.MigrateAsync();
    }


    /// <summary>
    /// convention : databaseName + Key
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static string GetConnectionStringInConfiguration(IConfiguration configuration)
    {
        string connectionStringKey = $"northwestConnectionString";

        string connectionString = configuration.GetSection(connectionStringKey).Value
                                  ?? throw new Exception(
                                      $"Connection string with key {connectionStringKey} not found in Configuration");

        return connectionString;
    }
}