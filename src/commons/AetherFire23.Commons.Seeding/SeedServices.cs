using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System.Reflection;

namespace AetherFire23.Commons.Seeding;

public static class SeedServices
{
    /// <summary>
    /// Safe to use because it is meant to be used by a single Api, not shared between tests 
    /// </summary>
    public static IEnumerable<Type> Seeded;

    /// <summary>
    /// Adds seed services in the specified assembly to the service collection. 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="assembly"></param>
    public static void AddSeedServices(this IServiceCollection serviceCollection, Assembly assembly)
    {
        Seeded = ScanSeedersInAssembly(assembly);

        foreach (Type type in Seeded)
        {
            serviceCollection.AddScoped(type);
        }
    }

    public static void ExecuteSeedFromArgs(this IServiceProvider serviceProvider, string[] args)
    {
        string seedName = args[args.IndexOf("--scenario") + 1];

        ExecuteSeedFromSeedName(serviceProvider, seedName);
    }

    public static void ExecuteSeedFromSeedName(this IServiceProvider serviceProvider, string seedName)
    {
        var seedServiceType = Seeded.Single(x => x.Name == seedName);

        using (var scope = serviceProvider.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<ILogger<Program>>()
                .LogInformation($"Executing {seedName} as seed. ");

            var s = (ISeeder)scope.ServiceProvider.GetRequiredService(seedServiceType);
            s.SetupSeeding().Wait();
        }
    }

    public static IEnumerable<Type> ScanSeedersInAssembly(Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => typeof(ISeeder).IsAssignableFrom(t) && !t.IsInterface);

        return types;
    }
}