using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

/// <summary>
/// Contains a name + a list of given requirements for an action. 
/// </summary>
public abstract class ActionAvailabilityBase
{
    /// <summary>
    /// Eventually realized by reflection ? 
    /// </summary>
    public required string ActionName { get; set; }

    public List<ActionRequirement> ActionRequirements { get; set; }

    public bool CanExecute
    {
        get { return this.ActionRequirements.All(a => a.IsFulfilled); }
    }
}