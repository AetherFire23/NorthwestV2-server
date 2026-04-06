using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features;
using NorthwestV2.Features.Installation;
using NorthwestV2.Infrastructure.Contexts;

namespace NorthwestV2.Compose;

public static class AppComposer
{
    public static void ComposeApplication(IServiceCollection serviceCOllection, IConfiguration configuration)
    {
        NorthwestContextInstaller.Install(serviceCOllection, configuration);
        InfraInstaller.Install(serviceCOllection);
        MediatorInstaller.Install(serviceCOllection);
        DomainInstaller.Install(serviceCOllection, configuration);
        ApplicationInstaller.Install(serviceCOllection);
    }

    public static async Task Initialize(IServiceProvider serviceProvider)
    {
         await NorthwestContextInstaller.Initialize(serviceProvider);
    }
}