using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace AetherFire23.ERP.Domain.Features.Actions.General.Combat;

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