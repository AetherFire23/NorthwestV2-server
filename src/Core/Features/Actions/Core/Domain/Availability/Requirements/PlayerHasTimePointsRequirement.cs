using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;

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