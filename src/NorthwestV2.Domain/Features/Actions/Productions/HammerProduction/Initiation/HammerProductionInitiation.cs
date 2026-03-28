using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;

public class HammerProductionInitiation
{
    public InstantActionAvailability DetermineAvailability()
    {
        InstantActionAvailability instant = new InstantActionAvailability()
        {
            ActionName = ActionNames.HammerProductionInitiation,
            DisplayName = "Initiate hammer production",
            ActionRequirements = []
        };

        return instant;
    }
}