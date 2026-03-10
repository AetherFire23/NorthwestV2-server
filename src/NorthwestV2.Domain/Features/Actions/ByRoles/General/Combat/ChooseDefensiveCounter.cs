using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.Feature.Availability.WithTargets;
using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;

public class ChooseDefensiveCounter
{
    private static readonly List<ActionTarget> DefensiveCountersAsTargets = Enum.GetValues<DefensiveCounters>()
        .Select(x =>
            new ActionTarget()
            {
                Value = x.ToString(),
                Name = x.ToString(),
                TargetId = Guid.Empty
            }).ToList();


    public ActionWithTargetsAvailability GetAvailability(Player caster, string displayName)
    {
        // TODO: Test if this still works if you give no action requirement (becuase there is none, justa fulfilled one)
        return new ActionWithTargetsAvailability()
        {
            ActionName = displayName,
            TargetSelectionPrompts = [CreateDefensiveCountersSelectionPrompt()],
            ActionRequirements = [],
        };
    }

    private TargetSelectionPrompt CreateDefensiveCountersSelectionPrompt()
    {
        return new TargetSelectionPrompt()
        {
            Description = "Pick a defensive counter.",
            ValidTargets = DefensiveCountersAsTargets,
        };
    }

    // TODO: Write unit tests 
    // TODO: Actually not domain logic here. Would be in the Application Layer of this service. 
    /// <summary>
    /// Sets defensive counter on the player 
    /// </summary>
    public void Execute(Player player, List<List<ActionTarget>> actionTargets)
    {
        DefensiveCounters defensiveCounter = ExtractDefensiveCounterFromActionTargets(actionTargets);

        player.DefensiveCounter = defensiveCounter;
    }

    private DefensiveCounters ExtractDefensiveCounterFromActionTargets(List<List<ActionTarget>> actionTargets)
    {
        if (actionTargets.Count > 0)
        {
            throw new Exception("Should not exceed 1 screen wth");
        }

        ActionTarget? target = actionTargets.FirstOrDefault()?.FirstOrDefault();

        if (target?.Value is null)
        {
            throw new Exception("Is null exception. ");
        }

        // Expects an enum.ToString()
        DefensiveCounters defensiveCounters = Enum.Parse<DefensiveCounters>(target.Value);

        return defensiveCounters;
    }
}