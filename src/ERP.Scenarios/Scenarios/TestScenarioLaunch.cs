using AetherFire23.Commons.Scenarios;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace ERP.Scenarios.Scenarios;

[Scenario("SeededCompany")]
public class TestScenarioLaunch : ScenarioBase
{
    public override async Task RunScenario(IServiceScope scope, IPlaywright playwright)
    {
        var p = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = false,
        });

        var s = await p.NewPageAsync();

        // FRONTEND URL LOCATI
        await s.GotoAsync("http://localhost:5173");
    }
}