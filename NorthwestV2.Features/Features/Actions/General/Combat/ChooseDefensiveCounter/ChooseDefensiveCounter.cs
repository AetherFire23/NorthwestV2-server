using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.General.Combat.ChooseDefensiveCounter;

public class ChooseDefensiveCounter
{

    /// <summary>
    /// Sets defensive counter on the player 
    /// </summary>
    public void Execute(Player player, DefensiveCounters defensiveCounters)
    {
        player.DefensiveCounter = defensiveCounters;
    }


    public TargetSelectionPrompt CreatePromptOfDefensiveCounters()
    {
        List<ActionTarget> actionTargets = Enum.GetValues<DefensiveCounters>()
            .Select(x =>
                new ActionTarget()
                {
                    Value = x.ToString(),
                    Name = x.ToString(),
                    TargetId = Guid.Empty
                }).ToList();

        var promptOfDefensiveCounterTypes = new TargetSelectionPrompt()
        {
            Description = "Choose a defensive target stance.",
            ValidTargets = actionTargets
        };

        return promptOfDefensiveCounterTypes;
    }
}