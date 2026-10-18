using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.Features.Actions.ActDeDebg.Instant;
using NorthwestV2.Features.Features.Actions.ActDeDebg.Targets;
using NorthwestV2.Features.Features.Actions.Core.Application;
using NorthwestV2.Features.Features.Actions.General.Combat;
using NorthwestV2.Features.Features.Actions.General.Combat.ChooseDefensiveCounter;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat;
using NorthwestV2.Features.Features.Actions.General.Movement;
using NorthwestV2.Features.Features.Actions.Productions;
using NorthwestV2.Features.Features.Actions.Productions.CancelProduction;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Initiation;
using NorthwestV2.Features.UseCases.Authentication.Login.Tokens;

namespace NorthwestV2.Features.Installation;

public class ApplicationInstaller
{
    public static void Install(IServiceCollection serviceCollection)
    {
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