using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

public class PlayerHasTimePointsRequirement : ActionRequirement
{
    private PlayerHasTimePointsRequirement(string description, bool isFulfilled) : base(description, isFulfilled)
    {
    }

    public static PlayerHasTimePointsRequirement Create(Player player, int timePoints)
    {
        bool isEnough = player.ActionPoints >= timePoints;
        string description = $"Player has sufficient timepoints: {player.ActionPoints}/{timePoints}";

        PlayerHasTimePointsRequirement tp = new PlayerHasTimePointsRequirement(description, isEnough);
        return tp;
    }
}