namespace NorthwestV2.Integration.UseCases.GameActions.Queries.GetActions.ReturnValue;

public class ActionRequirement
{
    public required string Description { get; set; }
    public bool IsFulfilled { get; set; }
}