using AetherFire23.Commons.Composition;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameInitialization;
using AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;
using AetherFire23.ERP.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            return new PlayerFactory(randomProvider, initializers);
        });
        serviceCollection.AddScoped<IRandomProvider, RealRandom>();
    }
}