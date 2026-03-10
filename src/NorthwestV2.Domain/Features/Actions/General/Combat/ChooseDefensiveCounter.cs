using AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;
using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.General.Combat;

public class ChooseDefensiveCounter
{
    public List<DefensiveCounters> GetValidTargets()
    {
        return Enum.GetValues<DefensiveCounters>().ToList();
    }


    /// <summary>
    /// Sets defensive counter on the player 
    /// </summary>
    public void Execute(Player player, DefensiveCounters defensiveCounters)
    {
        player.DefensiveCounter = defensiveCounters;
    }
}