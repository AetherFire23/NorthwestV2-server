using AetherFire23.Commons.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application;
using NorthwestV2.Practical;
using NorthwestV2.Practical.Repositories;

namespace NorthwestV2.Infrastructure.Contexts;

public class TestInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<WarehouseRepository>();
        serviceCollection.AddScoped<RequestContextService>();
    }
}