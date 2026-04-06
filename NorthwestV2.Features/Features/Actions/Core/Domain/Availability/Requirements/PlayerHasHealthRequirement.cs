using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;

public class PlayerHasHealthRequirement : ActionRequirement
{
    private PlayerHasHealthRequirement(string description, bool isFulfilled) : base(description, isFulfilled)
    {
    }

    public static PlayerHasHealthRequirement Create(Player player, int minimumHealth)
    {
        bool isEnough = player.Health > minimumHealth;
        string description = $"Player has sufficient health: {player.Health}/{minimumHealth + 1}+";

        return new PlayerHasHealthRequirement(description, isEnough);
    }
}
