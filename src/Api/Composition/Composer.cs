using NorthwestV2.Contexts;
using NorthwestV2.Features;
using NorthwestV2.Features.Actions.Core.Application;
using NorthwestV2.Installation;

namespace NorthwestV2.Api.Composition;

public static class Composer
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        NorthwestContextInstaller.Install(services, configuration);
        InfraInstaller.Install(services);
        MediatorInstaller.Install(services);
        DomainInstaller.Install(services, configuration);
        ApplicationInstaller.Install(services);
    }

    public static async Task InitializeServices(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        await NorthwestContextInstaller.Initialize(serviceProvider);
        serviceProvider.GetRequiredService<ActionCatalog>().Initialize(serviceProvider);
    }
}
