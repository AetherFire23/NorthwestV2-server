using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using NorthwestV2.Application.Features.Actions.Core.Bases;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class ActionDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ActionKinds Kind { get; set; }
    public required List<ActionRequirement> Requirements { get; set; }
    public required List<TargetSelectionPrompt> Prompts { get; set; }

    public static ActionDto FromInstant(InstantActionAvailability availability)
    {
        ActionDto acti = new ActionDto()
        {
            Requirements = availability.ActionRequirements,
            Description = "Not set yet",
            Kind = ActionKinds.Instant,
            Name = availability.ActionName,
            Prompts = new List<TargetSelectionPrompt>(), // Always empty
        };

        return acti;
    }

    public static ActionDto FromTarget(ActionWithTargetsAvailability availability)
    {
        ActionDto acti = new ActionDto()
        {
            Requirements = availability.ActionRequirements,
            Description = "Not set yet",
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