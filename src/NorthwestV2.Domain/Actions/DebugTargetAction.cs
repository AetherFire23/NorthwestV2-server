using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace AetherFire23.ERP.Domain.Actions;

public class DebugTargetAction
{
    public ActionWithTargetsAvailability GetAvailability(Player caster, List<Player> players)
    {
        ActionRequirement actionRequirement = new ActionRequirement()
        {
            Description = "Action points requirement.",
            IsFulfilled = caster.ActionPoints > 2,
        };

        List<TargetSelectionPrompt> targetSelectionPrompts = GetValidTargets(players);

        return new ActionWithTargetsAvailability([actionRequirement])
        {
            ActionName = "Debug Target Action",
            TargetSelectionPrompts = targetSelectionPrompts,
        };
    }

    private new List<TargetSelectionPrompt> GetValidTargets(List<Player> players)
    {
        return [BuildFirstTargetScreen(), BuildSecondTargetScreen(players)];
    }

    private TargetSelectionPrompt BuildFirstTargetScreen()
    {
        ActionTarget oneDamage = new ActionTarget
        {
            Value = "1",
        };
        ActionTarget twoDamage = new ActionTarget
        {
            Value = "2",
        };
        List<ActionTarget> targets = new List<ActionTarget>([oneDamage, twoDamage]);

        return new TargetSelectionPrompt()
        {
            Description = "Select the amount of damage you want to do",
            Targets = targets,
            Max = 1
        };
    }

    private TargetSelectionPrompt BuildSecondTargetScreen(List<Player> players)
    {
        ActionTarget oneDamage = new ActionTarget
        {
            TargetId = players[0].Id
        };
        ActionTarget twoDamage = new ActionTarget
        {
            TargetId = players[1].Id,
        };

        List<ActionTarget> targets = new([oneDamage, twoDamage]);

        return new TargetSelectionPrompt()
        {
            Description = "Select the amount of damage you want to do",
            Targets = targets
        };
    }
}