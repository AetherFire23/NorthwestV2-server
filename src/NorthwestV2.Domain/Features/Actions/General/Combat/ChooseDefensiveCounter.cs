using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;
using AetherFire23.ERP.Domain.Actions.Feature.Availability.WithTargets;
using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.General.Combat;

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

    /// <summary>
    /// Sets defensive counter on the player 
    /// </summary>
    public void Execute(Player player, DefensiveCounters defensiveCounters)
    {
        player.DefensiveCounter = defensiveCounters;
    }
}