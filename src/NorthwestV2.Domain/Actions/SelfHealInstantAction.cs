using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Actions;

public class SelfHealInstantAction
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

    public void Execute(Player player)
    {
        player.Health += 2;
    }
}