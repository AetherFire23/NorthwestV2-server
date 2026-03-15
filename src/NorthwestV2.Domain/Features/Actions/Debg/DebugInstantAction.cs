using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;

namespace AetherFire23.ERP.Domain.Features.Actions.Debug;

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
            ActionRequirements = [actionRequirement]
        };

        return vs;
    }
}