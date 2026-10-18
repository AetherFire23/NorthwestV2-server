namespace NorthwestV2.Api.Scenarios;

[AttributeUsage(AttributeTargets.Class)]
public class ScenarioAttribute : Attribute
{
    public string ScenarioName { get; init; }

    public ScenarioAttribute(string scenarioName)
    {
        ScenarioName = scenarioName;
    }
}