using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class CombatActionApp : ActionWithTargetsBase
{
        
    public CombatActionApp(NorthwestContext context, string actionName) : base(context, actionName)
    {
    }

    public override Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        throw new NotImplementedException();
    }

    public override Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}