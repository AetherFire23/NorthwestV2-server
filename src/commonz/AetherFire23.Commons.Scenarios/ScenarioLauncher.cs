using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace AetherFire23.Commons.Scenarios;

public class ScenarioLauncher
{
    public async Task LaunchScenario(IServiceScope scope, string scenarioName)
    {
        // get scenario classes
        Type scenario = typeof(ScenarioLauncher).Assembly.GetTypes()
            .Where(t => !t.IsAbstract
                        && !t.IsInterface
                        && typeof(ScenarioBase).IsAssignableFrom(t)
                        && t.GetCustomAttribute<ScenarioAttribute>() is not null
            )
            // With the appropriate attribute
            .First(t => t.GetCustomAttribute<ScenarioAttribute>().ScenarioName == scenarioName);

        // prepare Playwright browser 

        IPlaywright playwright = await Playwright.CreateAsync();

        // Run The scenario
        await ((ScenarioBase)Activator.CreateInstance(scenario)).RunScenario(scope, playwright);
    }
}