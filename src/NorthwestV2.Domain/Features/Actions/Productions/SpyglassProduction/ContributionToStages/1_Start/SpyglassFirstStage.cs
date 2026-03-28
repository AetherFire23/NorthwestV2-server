using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._1_Start;

/// <summary>
/// Represents the first production stage action for crafting a spyglass.
/// Handles availability calculation for players to contribute to this stage.
/// </summary>
/// <remarks>
/// Requires the player to be in the Workshop room and hold a Scrap item.
/// Inherits from <see cref="Stage"/> which defines room and item requirements.
/// </remarks>
public class SpyglassFirstStage : Stage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpyglassFirstStage"/> class,
    /// specifying the required room and items for this stage.
    /// </summary>
    public SpyglassFirstStage() : base(RoomEnum.Workshop, [ItemTypes.Scrap])
    {
        
    }

    /// <summary>
    /// Calculates the availability of this production stage action for a given player.
    /// </summary>
    /// <param name="player">The player to check availability for.</param>
    /// <returns>An <see cref="InstantActionAvailability"/> containing the action name, display name, and fulfilled requirements.</returns>
    public InstantActionAvailability CalculateAvailability(Player player)
    {
        List<ActionRequirement> spyglassInitiationRequirement =
            GetPlayerInRoomAndPlayerHoldsItemRequirements(player);

        InstantActionAvailability instant = new InstantActionAvailability()
        {
            ActionName = ActionNames.SpyglassProductionStart,
            DisplayName = "Initiate spyglass production",
            ActionRequirements = spyglassInitiationRequirement,
        };

        return instant;
    }

    /// <summary>
    /// Builds the list of requirements the player must meet to contribute to this stage.
    /// </summary>
    /// <param name="player">The player to evaluate requirements for.</param>
    /// <returns>A list of <see cref="ActionRequirement"/> objects that must all be fulfilled.</returns>
    private List<ActionRequirement> GetPlayerInRoomAndPlayerHoldsItemRequirements(Player player)
    {
        PlayerInRoomRequirement playerInRoomRequirement = new PlayerInRoomRequirement(player, RoomEnum.Armory);

        ActionRequirement playerHoldsItemRequirement = new ActionRequirement()
        {
            Description = "Player holds a Scrap Item.",
            IsFulfilled = player.Inventory.Items.Any(x => x.ItemType == ItemTypes.Scrap),
        };
        return [playerInRoomRequirement, playerHoldsItemRequirement];
    }
}