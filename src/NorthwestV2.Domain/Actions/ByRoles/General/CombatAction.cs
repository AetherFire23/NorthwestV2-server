using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.Feature.Availability.Instant;
using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Actions.ByRoles.General;

public class CombatAction
{
    public InstantActionAvailability GetAvailability(Player caster)
    {
        ActionRequirement actionRequirement = new()
        {
            Description = "Heals for 2 points ",
            IsFulfilled = caster.ActionPoints > 2,
        };

        InstantActionAvailability vs = new()
        {
            ActionName = ActionNames.InstantHeal,
            ActionRequirements = [actionRequirement]
        };

        return vs;
    }
}