using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.ActDeDebg.Targets;

public class DebugTargetAction
{
    public ActionWithTargetsAvailability GetAvailability(Player caster, List<Player> players)
    {
        ActionRequirement actionRequirement = new ActionRequirement()
        {
            Description = "Action points requirement.",
            IsFulfilled = caster.ActionPoints > 2,
        };

        ActionWithTargetsAvailability debugTargetAvailability = new ActionWithTargetsAvailability()
        {
            ActionName = ActionNames.DebugWithTargets,
            TargetSelectionPrompts = GetValidTargets(players),
            ActionRequirements = [actionRequirement],
            DisplayName = ActionNames.DebugWithTargets
        };

        return debugTargetAvailability;
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
            Name = "One"
        };
        ActionTarget twoDamage = new ActionTarget
        {
            Value = "2",
            Name = "Two"
        };
        List<ActionTarget> targets = new List<ActionTarget>([oneDamage, twoDamage]);

        return new TargetSelectionPrompt()
        {
            Description = "Select the amount of damage you want to do",
            ValidTargets = targets,
            Max = 1
        };
    }

    private TargetSelectionPrompt BuildSecondTargetScreen(List<Player> players)
    {
        ActionTarget oneDamage = new ActionTarget
        {
            TargetId = players[0].Id,
            Name = players.First().Role.ToString()
        };
        ActionTarget twoDamage = new ActionTarget
        {
            TargetId = players[1].Id,
            Name = players[1].Role.ToString()
        };

        List<ActionTarget> targets = new([oneDamage, twoDamage]);

        return new TargetSelectionPrompt()
        {
            Description = "Select the amount of damage you want to do",
            ValidTargets = targets
        };
    }
}