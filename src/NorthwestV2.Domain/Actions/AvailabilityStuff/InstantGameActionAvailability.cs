using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;


public class InstantGameActionAvailability : ActionAvailabilityBase
{
    public InstantGameActionAvailability(List<ActionRequirement> actionRequirements) : base(actionRequirements)
    {
    }
}