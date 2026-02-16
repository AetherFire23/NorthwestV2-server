using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.Commons.Scenarios;

public static class ScenarioConfigurator
{
    public static void InstallScenarioLauncher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ScenarioLauncher>();
    }

    /// <summary>
    /// Leave as fire-and-forget async call otherwise it will wait for the browser to close
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="scenarionName"></param>
    public static async Task LaunchScenarioBrowser(this IServiceProvider serviceProvider, string scenarionName)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        ScenarioLauncher scenarioLauncher = scope.ServiceProvider.GetRequiredService<ScenarioLauncher>();

        await scenarioLauncher.LaunchScenario(scope, scenarionName);
    }
}