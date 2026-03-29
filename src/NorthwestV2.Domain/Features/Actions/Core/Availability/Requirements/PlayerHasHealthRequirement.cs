using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

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
