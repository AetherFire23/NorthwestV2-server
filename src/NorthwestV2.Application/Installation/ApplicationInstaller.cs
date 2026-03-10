using AetherFire23.Commons.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.ByRoles.Debug;
using NorthwestV2.Application.Features.Actions.Core;
using NorthwestV2.Application.Features.Actions.Debug;
using NorthwestV2.Application.Features.Actions.General.Combat;

namespace NorthwestV2.Application.Installation;

public class ApplicationInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<DebugInstantActionApp>();
        serviceCollection.AddScoped<DebugTargetsActionApp>();
        serviceCollection.AddScoped<ActionServices>();
        serviceCollection.AddScoped<ChooseDefensiveCounterApp>();
        serviceCollection.AddScoped<CombatActionApp>();
    }
}