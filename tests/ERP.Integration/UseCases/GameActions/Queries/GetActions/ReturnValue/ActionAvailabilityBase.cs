namespace NorthwestV2.Integration.UseCases.GameActions.Queries.GetActions.ReturnValue;

public abstract class ActionAvailabilityBase
{
    /// <summary>
    /// Eventually realized by reflection ? 
    /// </summary>
    public required string ActionName { get; set; }

    public List<ActionRequirement> ActionRequirements { get; set; } = [];

    public bool CanExecute
    {
        get
        {
            return ActionRequirements.Count != 0 && this.ActionRequirements.All(a => a.IsFulfilled);
        }
    }
}