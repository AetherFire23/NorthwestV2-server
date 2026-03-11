using AetherFire23.Commons.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Infrastructure.Repositories;
using NorthwestV2.Practical;

namespace NorthwestV2.Infrastructure.Contexts;

public class TestInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<RequestContextService>();
    }
}