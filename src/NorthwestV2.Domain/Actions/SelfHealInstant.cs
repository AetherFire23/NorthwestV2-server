using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace AetherFire23.ERP.Domain.Actions;

public class SelfHealInstant
{
    // The goal here is to keep the parameters unit-tested.

    public InstantGameActionAvailability GetAvailability(Player caster)
    {
        ActionRequirement actionRequirement = new ActionRequirement()
        {
            Description = "Action points requirement",
            IsFulfilled = caster.ActionPoints > 2,
        };

        InstantGameActionAvailability vs = new InstantGameActionAvailability([actionRequirement])
        {
            ActionName = "Self heal instant",
        };

        return vs;
    }
}