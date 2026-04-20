using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.Commons.Scenarios;

public static class ScenarioConfigurator
{
    public static void InstallScenarioLauncher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ScenarioLauncher>();
    }

    public static async Task LaunchScenarioBrowser(this IServiceProvider serviceProvider, string scenarionName)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        ScenarioLauncher scenarioLauncher = scope.ServiceProvider.GetRequiredService<ScenarioLauncher>();

        await scenarioLauncher.LaunchScenario(scope ,scenarionName);
    }
}