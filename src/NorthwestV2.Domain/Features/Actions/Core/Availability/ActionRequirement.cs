using System.ComponentModel.DataAnnotations;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability;

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
    public required string Description { get; set; }

    /// <summary>
    /// Indicates whether this requirement is currently satisfied.
    /// </summary>
    public bool IsFulfilled { get; set; }

    public static List<ActionRequirement> None = [NoCondition];

    private static ActionRequirement NoCondition = new ActionRequirement()
    {
        Description = "There are no preconditions to this action.",
        IsFulfilled = true,
    };

    public override string ToString()
    {
        return $"{this.Description} : {this.IsFulfilled}";
    }
}