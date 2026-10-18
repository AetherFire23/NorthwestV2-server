namespace NorthwestV2.Scenarios;

[AttributeUsage(AttributeTargets.Class)]
public class ScenarioAttribute : Attribute
{
    public string ScenarioName { get; init; }

    public ScenarioAttribute(string scenarioName)
    {
        ScenarioName = scenarioName;
    }
}