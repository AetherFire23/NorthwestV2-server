using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.ActDeDebg.Instant;

/// <summary>
/// 
/// </summary>
public class DebugInstantAction
{
    // The goal here is to keep the parameters unit-tested.

    public InstantActionAvailability GetAvailability(Player caster)
    {
        ActionRequirement actionRequirement = new ActionRequirement()
        {
            Description = "Heals for 2 points ",
            IsFulfilled = caster.ActionPoints > 2,
        };

        InstantActionAvailability vs = new InstantActionAvailability()
        {
            ActionName = ActionNames.InstantHeal,
            ActionRequirements = [actionRequirement],
            DisplayName = ActionNames.InstantHeal
        };

        return vs;
    }
}