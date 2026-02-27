using AetherFire23.Commons.Scenarios;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace NorthwestV2.Scenarios.Scenarios;

[Scenario("SeededCompany")]
public class TestScenarioLaunch : ScenarioBase
{
    public override async Task RunScenario(IServiceScope scope, IPlaywright playwright)
    {
        IBrowser p = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = false,
        });

        IPage s = await p.NewPageAsync();

        // FRONTEND URL LOCATI
        await s.GotoAsync("http://localhost:5173");
    }
}