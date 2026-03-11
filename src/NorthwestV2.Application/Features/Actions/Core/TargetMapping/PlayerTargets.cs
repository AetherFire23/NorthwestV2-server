using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;

namespace NorthwestV2.Application.Features.Actions.Core.TargetMapping;

public class PlayerTargets
{
    public List<ActionTarget> Convert(List<Player> players)
    {
        List<ActionTarget> actionTargets = new List<ActionTarget>();

        foreach (Player player in players)
        {
            actionTargets.Add(new ActionTarget()
            {
                Name = player.Role.ToString(),
                TargetId = player.Id
            });
        }

        return actionTargets;
    }
}