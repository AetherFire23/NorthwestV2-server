using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class CombatActionApp : ActionWithTargetsBase
{
    public CombatActionApp(NorthwestContext context) : base(context, ActionNames.CombatAction)
    {
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        return new ActionWithTargetsAvailability
        {
            ActionName = this.ActionName,
            ActionRequirements = ActionRequirement.None,
            TargetSelectionPrompts =
            [
                new TargetSelectionPrompt
                {
                    Description = "Picka a player Target",
                    ValidTargets =
                    [
                        new ActionTarget
                        {
                            Name = "Convert all player to targets here"
                        }
                    ],
                }
            ]
        };
    }

    public override Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}