using Microsoft.Playwright;

namespace NorthwestV2.Api.Scenarios;

public abstract class ScenarioBase
{
    public abstract Task RunScenario(IServiceScope scope, IPlaywright playwright);
}