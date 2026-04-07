using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;

namespace NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

public class ActionDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ActionKinds Kind { get; set; }
    public required List<ActionRequirement> Requirements { get; set; }
    public required List<TargetSelectionPrompt> Prompts { get; set; }

    public bool IsExecutable()
    {
        bool isExecutable = this.Requirements.All(x => x.IsFulfilled);
        return isExecutable;
    }

    public static ActionDto FromInstant(InstantActionAvailability availability)
    {
        ActionDto acti = new()
        {
            Requirements = availability.ActionRequirements,
            Description = availability.DisplayName,
            Kind = ActionKinds.Instant,
            Name = availability.ActionName,
            Prompts = new List<TargetSelectionPrompt>(), // Always empty
        };

        return acti;
    }

    public static ActionDto FromTarget(ActionWithTargetsAvailability availability)
    {
        ActionDto acti = new()
        {
            Requirements = availability.ActionRequirements,
            Description = availability.DisplayName,
            Kind = ActionKinds.WithTargets,
            Name = availability.ActionName,
            Prompts = availability.TargetSelectionPrompts, // Always empty
        };

        return acti;
    }

    public override string ToString()
    {
        return this.Name;
    }
}