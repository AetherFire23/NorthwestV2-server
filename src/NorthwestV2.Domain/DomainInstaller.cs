using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Debug;
using AetherFire23.ERP.Domain.Features.Actions.General.Combat;
using AetherFire23.ERP.Domain.GameStart;
using AetherFire23.ERP.Domain.GameStart.RoleInitializations;
using AetherFire23.ERP.Domain.GameStart.RoleInitializations.PlayerInitializers;
using AetherFire23.ERP.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AetherFire23.ERP.Domain;

public static class DomainInstaller
{
    public static void Install(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<NorthwestDomainService>();
        serviceCollection.AddScoped<RoomFactory>();
        serviceCollection.AddScoped<GameActionsWithTargetsValidator>();
        serviceCollection.AddScoped<CombatAction>();
        serviceCollection.AddScoped<DebugTargetAction>();
        serviceCollection.AddScoped<DebugInstantAction>();
 

        serviceCollection.AddScoped<IRandomProvider, RealRandom>();
        InstallActionServices(serviceCollection);
        RegisterPlayerFactory(serviceCollection);
        InstallRoleServices(serviceCollection);
    }

    private static void InstallActionServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ChooseDefensiveCounter>();
    }

    private static void InstallRoleServices(IServiceCollection services)
    {
        services.AddScoped<CaptainRoleInitializer>();
        services.AddScoped<BruteRoleInitializer>();
        services.AddScoped<ChaplainRoleInitializer>();
        services.AddScoped<EngineerRoleInitializer>();
        services.AddScoped<SentryRoleInitializer>();
        services.AddScoped<RangerRoleInitializer>();
        services.AddScoped<DoctorRoleInitializer>();
        services.AddScoped<CookRoleInitializer>();
        services.AddScoped<MarineRoleInitializer>();
        services.AddScoped<SapperRoleInitializer>();
        services.AddScoped<Scholar>();
        services.AddScoped<Quartermaster>();
    }

    /// <summary>
    /// A domain should never know about service providers because it has to be unit tested.
    /// However I wouldnt do this in an applicaiton layer ( I would use reflection )
    /// Also im not doing reflection here because you just do it once and it simply avoid reflection. 
    /// </summary>
    /// <param name="serviceCollection"></param>
    private static void RegisterPlayerFactory(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<PlayerFactory>(s =>
        {
            CaptainRoleInitializer captainRole = s.GetRequiredService<CaptainRoleInitializer>();
            BruteRoleInitializer bruteRoleInitializer = s.GetRequiredService<BruteRoleInitializer>();
            ChaplainRoleInitializer chaplainRoleInitializer = s.GetRequiredService<ChaplainRoleInitializer>();
            EngineerRoleInitializer engineerRoleInitializer = s.GetRequiredService<EngineerRoleInitializer>();
            SentryRoleInitializer entryRoleInitializer = s.GetRequiredService<SentryRoleInitializer>();
            RangerRoleInitializer rangerRoleInitializer = s.GetRequiredService<RangerRoleInitializer>();
            DoctorRoleInitializer doctorRoleInitializer = s.GetRequiredService<DoctorRoleInitializer>();
            CookRoleInitializer cookRoleInitializer = s.GetRequiredService<CookRoleInitializer>();
            MarineRoleInitializer marineRoleInitializer = s.GetRequiredService<MarineRoleInitializer>();
            SapperRoleInitializer sapperRoleInitializer = s.GetRequiredService<SapperRoleInitializer>();
            Scholar scholarRoleInitializer = s.GetRequiredService<Scholar>();
            Quartermaster quartermaster = s.GetRequiredService<Quartermaster>();
            IRandomProvider randomProvider = s.GetRequiredService<IRandomProvider>();
            IEnumerable<RoleInitializer> initializers =
            [
                captainRole,
                bruteRoleInitializer,
                chaplainRoleInitializer,
                engineerRoleInitializer,
                entryRoleInitializer,
                rangerRoleInitializer,
                cookRoleInitializer,
                marineRoleInitializer,
                sapperRoleInitializer,
                scholarRoleInitializer,
                quartermaster,
                doctorRoleInitializer
            ];
            ILogger<PlayerFactory> logger = s.GetRequiredService<ILogger<PlayerFactory>>();
            return new PlayerFactory(randomProvider, initializers, logger);
        });
    }
}