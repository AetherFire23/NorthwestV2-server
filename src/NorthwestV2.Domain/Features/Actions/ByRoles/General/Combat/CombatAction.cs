using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.Feature.Availability.Instant;
using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;

public class CombatAction
{
    /*
     * Attack types
     *  - Kill
     *  - Stun
     *
     * Defensive Counters ( Defined in another action
     *  - Default vs stun
     *  - Override - kill counter
     *
     * Stances
     *  - Hit and run -> Disengages immediately if strength gap is too low; attacker avoids damage and likely avoids identification.
     *  - Push Hard -> Attacker continues until losing inevitable, then flees, if no kill achieved, they are identified.
     *
     */

    public ActionWithTargetsAvailability GetAvailability(Player caster, string displayName)
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
        
        // TODO: Not finished at all here. 

        return new ActionWithTargetsAvailability()
        {
            ActionName = displayName,
        };
    }

    // how to calcualte Strength 
}