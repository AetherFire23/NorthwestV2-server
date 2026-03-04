using AetherFire23.Commons.Composition;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameInitialization;
using AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;
using AetherFire23.ERP.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AetherFire23.ERP.Domain;

public class DomainInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<NorthwestDomainService>();
        serviceCollection.AddScoped<RoomFactory>();
        serviceCollection.AddScoped<PlayerFactory>(s =>
        {
            var captainRole = s.GetRequiredService<CaptainRoleInitializer>();
            var randomProvider = s.GetRequiredService<IRandomProvider>();
            IEnumerable<CaptainRoleInitializer> initializers = [captainRole];
            ILogger<PlayerFactory> logger = s.GetRequiredService<ILogger<PlayerFactory>>();
            return new PlayerFactory(randomProvider, initializers, logger);
        });
        serviceCollection.AddScoped<IRandomProvider, RealRandom>();

        InstallRoleServices(serviceCollection);
    }

    private void InstallRoleServices(IServiceCollection services)
    {
        services.AddScoped<CaptainRoleInitializer>();
        services.AddScoped<BruteRoleInitializer>();
        services.AddScoped<ChaplainRoleInitializer>();
        services.AddScoped<EngineerRoleInitializer>();
        services.AddScoped<SentryRoleInitializer>();
        services.AddScoped<RangerRoleInitializer>();
        services.AddScoped<CaptainRoleInitializer>();
        services.AddScoped<DoctorRoleInitializer>();
        services.AddScoped<CookRoleInitializer>();
        services.AddScoped<MarineRoleInitializer>();
    }
}