using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace AetherFire23.Commons.Scenarios;

public abstract class ScenarioBase
{
    public abstract Task RunScenario(IServiceScope scope, IPlaywright playwright);
}