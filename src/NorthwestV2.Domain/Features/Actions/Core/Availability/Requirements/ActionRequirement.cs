using System.ComponentModel.DataAnnotations;
using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

/// <summary>
/// Represents a single requirement that must be satisfied for an action
/// to be considered executable.
/// </summary>
/// <remarks>
/// Requirements are intentionally simple: each has a human-readable
/// <see cref="Description"/> and a boolean <see cref="IsFulfilled"/> flag.
/// More complex logic should be handled by higher-level evaluators.
/// </remarks>
public class ActionRequirement
{
    /// <summary>
    /// A human-readable explanation of what this requirement represents.
    /// Useful for UI feedback or debugging.
    /// </summary>
    [Required]
    public virtual string Description { get; set; }

    /// <summary>
    /// Indicates whether this requirement is currently satisfied.
    /// </summary>
    public virtual bool IsFulfilled { get; set; }


    public ActionRequirement()
    {
    }

    public ActionRequirement(string description, bool isFulfilled)
    {
        Description = description;
        IsFulfilled = isFulfilled;
    }
    public static implicit operator bool(ActionRequirement r) => r.IsFulfilled;


    private static ActionRequirement NoCondition = new ActionRequirement()
    {
        Description = "There are no preconditions to this action.",
        IsFulfilled = true,
    };

    public override string ToString()
    {
        return $"{this.Description} : {this.IsFulfilled}";
    }

    public static ActionRequirement CreateAnyOtherPlayerExistsInRoomRequirement(Player caster,
        List<Player> otherPlayersInSameRoom)
    {
        if (otherPlayersInSameRoom.Contains(caster))
        {
            throw new Exception("The other players in the same room should not contain the caster.");
        }

        ActionRequirement actionRequirement = new ActionRequirement()
        {
            Description = "Another player exists in the same room. bla bla bla ",
            IsFulfilled = otherPlayersInSameRoom.Count != 0
        };

        return actionRequirement;
    }
}