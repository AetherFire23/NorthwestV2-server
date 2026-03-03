using AetherFire23.Commons.Composition;
using AetherFire23.ERP.Domain.GameInitialization;
using AetherFire23.ERP.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.ERP.Domain;

public class DomainInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<NorthwestDomainService>();
        serviceCollection.AddScoped<PlayerFactory>();
        serviceCollection.AddScoped<RoomFactory>();
        serviceCollection.AddScoped<IRandomProvider, RealRandom>();
    }
}