using AetherFire23.Commons.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.ByRoles.Debug;
using NorthwestV2.Application.Features.Actions.Core;

namespace NorthwestV2.Application.Installation;

public class ApplicationInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<DebugInstantActionApp>();
        serviceCollection.AddScoped<DebugTargetsActionApp>();
        serviceCollection.AddScoped<ActionServices>();
    }
}