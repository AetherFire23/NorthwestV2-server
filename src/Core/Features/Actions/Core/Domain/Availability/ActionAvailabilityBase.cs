using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;

namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability;

/// <summary>
/// Represents the availability state of a game action, including its name
/// and the set of requirements that must be fulfilled before the action
/// can be executed.
/// </summary>
/// <remarks>
/// This class acts as a lightweight domain model for action gating logic.
/// 
/// <para>
/// <see cref="ActionRequirements"/> contains the list of conditions that
/// must be satisfied. The <see cref="CanExecute"/> property evaluates all
/// requirements and determines whether the action is currently allowed.
/// </para>
/// </remarks>
public abstract class ActionAvailabilityBase
{
    public required string ActionName { get; set; }

    public List<ActionRequirement> ActionRequirements { get; set; } =
    [
        new ActionRequirement
        {
            Description = " Undefined",
            IsFulfilled = true
        }
    ];

    public required string DisplayName { get; set; }


    public bool CanExecute
    {
        get
        {
            bool res = this.ActionRequirements.All(a => a.IsFulfilled);
            return res;
        }
    }

    public override string ToString()
    {
        return $"{this.ActionName}";
    }
}