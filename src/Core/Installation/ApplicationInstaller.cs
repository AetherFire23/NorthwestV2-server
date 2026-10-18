using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.Actions.ActDeDebg.Instant;
using NorthwestV2.Features.Actions.ActDeDebg.Targets;
using NorthwestV2.Features.Actions.Core.Application;
using NorthwestV2.Features.Actions.General.Combat.ChooseDefensiveCounter;
using NorthwestV2.Features.Actions.General.Combat.StartCombat;
using NorthwestV2.Features.Actions.General.Movement;
using NorthwestV2.Features.Actions.Productions.CancelProduction;
using NorthwestV2.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Actions.Productions.FishingPole.Initiation;
using NorthwestV2.Features.Actions.Productions.HammerProduction.Contribution;
using NorthwestV2.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Features.Actions.Productions.SpyglassProduction.Contribution;
using NorthwestV2.Features.Actions.Productions.SpyglassProduction.Initiation;
using NorthwestV2.UseCases.Authentication.Login.Tokens;

namespace NorthwestV2.Installation;

public class ApplicationInstaller
{
    public static void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(ActionCatalog.Scan(typeof(ActionCatalog).Assembly));
        serviceCollection.AddScoped<ActionServices>();
        serviceCollection.AddScoped<ChooseDefensiveCounterApp>();
        serviceCollection.AddScoped<CombatActionApp>();
        serviceCollection.AddScoped<DebugInstantActionApp>();
        serviceCollection.AddScoped<DebugTargetsActionApp>();
        serviceCollection.AddScoped<ChangeRoomApp>();
        serviceCollection.AddScoped<JwtTokenService>();
        serviceCollection.AddScoped<SpyglassProductionInitiationActionApp>();
        serviceCollection.AddScoped<SpyglassProductionContributionActionApp>();
        serviceCollection.AddScoped<CancelProductionActionApp>();
        serviceCollection.AddScoped<HammerProductionContributionApp>();

        serviceCollection.AddScoped<HammerProductionInitiationApp>();

        serviceCollection.AddScoped<FishingPoleInitiationApp>();
        serviceCollection.AddScoped<FishingPoleContributionApp>();
    }
}