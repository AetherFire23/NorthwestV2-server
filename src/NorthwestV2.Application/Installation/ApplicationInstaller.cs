using AetherFire23.Commons.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.UseCases.GameActions.Services;
using NorthwestV2.Application.UseCases.GameActions.Services.ActionBases;

namespace NorthwestV2.Application.Installation;

public class ApplicationInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<SelfHealInstantApp>();
        serviceCollection.AddScoped<DebugTargetsActionApp>();
        serviceCollection.AddScoped<ActionServices>();
    }
}