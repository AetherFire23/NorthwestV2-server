using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace NorthwestV2.Scenarios;

public abstract class ScenarioBase
{
    public abstract Task RunScenario(IServiceScope scope, IPlaywright playwright);
}