using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.Core.Entities;

/// <summary>
/// Base class for unfinished production items.
/// Unfinished items behave like normal items: they can be picked up, carried away, and left in other rooms.
/// They store both the locked items used to start the production and the accumulated Technical Points (TP) contributions.
/// Each production is divided into stages, each with its own TP quota and room requirement.
/// When all stages are complete, the production resolves and the finished item is given to the last TP contributor.
/// </summary>
public abstract class ProductionItemBase : ItemBase
{
    // TODO: Include a Stage 
    /// <summary>
    /// Items that were used to start the production and are now locked inside the unfinished item.
    /// These items are removed from the room's inventory and cannot be used until the production is cancelled or completed.
    /// </summary>
    public virtual List<CommonItemBase> LockedItems { get; set; } = [];

    /// <summary>
    /// Tracks the current stage progress and accumulated Technical Points (TP) for this production.
    /// Contains the contributions count, stage requirements, and whether the production is complete.
    /// </summary>
    public virtual StageContributionBase CurrentStageContribution { get; set; }

    /// <summary>
    /// There is serialization-deserialization stuff happening.
    /// We need constructors, but at least we can but them as protected / private
    /// </summary>
    protected ProductionItemBase()
    {
    }

    public ProductionItemBase(ItemTypes itemType, int carryValue, StageContributionBase initialStageContribution) :
        base(itemType, carryValue)
    {
        this.CurrentStageContribution = initialStageContribution;
    }

    /// <summary>
    /// Locks an item for use in the production, removing it from the room's inventory.
    /// The item becomes unavailable for other uses until the production is cancelled or completed.
    /// </summary>
    /// <param name="other">The item to lock for production.</param>
    /// <exception cref="Exception">Thrown when the item cannot be locked (not in inventory or already locked).</exception>
    public void LockForProduction(CommonItemBase other)
    {
        if (other.Inventory is null || other.Inventory.Items.Count == 0)
        {
            throw new Exception("Cannot remove from inventory if inventory is not loaded;");
        }

        if (other.IsLocked)
        {
            throw new Exception($"Cannot be locked twice - {other}");
        }

        // removes the item from the room's inventory ( exists only by virtue of being a locked item. of this object)
        other.IsLocked = true;
        other.Inventory.Items.Remove(other);

        this.LockedItems.Add(other);
    }

    /*
     * TODO: Add specialized TP.
     * The feature is kinda awkward because the points *coints* are tripled. So basically the requirement is dynamic :
     * If not of the given role; then the cost is 3. ( but it would just be increased by 1 ) to not disruptt the logic
     */
    /// <summary>
    /// Allows a player to contribute Time Points (TP) to advance the production.
    /// The player spends action points based on their role specialization (non-specialists pay 3x, Quartermaster pays 2x).
    /// Contributions are permanent - TP spent remains bound to the production until the game ends.
    /// If the current stage is completed, automatically advances to the next stage.
    /// If all stages are complete, the production is finalized and the finished item is given to this player.
    /// </summary>
    /// <param name="player">The player contributing TP.</param>
    /// <exception cref="Exception">Thrown when the player does not have enough action points.</exception>
    public void Contribute(Player player)
    {
        bool wouldHitPointsFallBelowZero = player.ActionPoints - 1 == -1;
        if (wouldHitPointsFallBelowZero)
        {
            throw new Exception("Player does not have enough points to contribute.");
        }

        int actionPointsCost = this.CurrentStageContribution.CalculateCostForContribution(player);
        player.ActionPoints -= actionPointsCost;

        // Increment the current stage by 1 
        this.CurrentStageContribution = this.CurrentStageContribution with
        {
            RequiredContributions = this.CurrentStageContribution.RequiredContributions + 1
        };

        bool isCurrentStadedEndedButCurrentIsNotComplete =
            this.CurrentStageContribution.IsStageEnded &&
            !CurrentStageContribution.IsProductionComplete;
        if (isCurrentStadedEndedButCurrentIsNotComplete)
        {
            /*
             * Is handled automatically by ef core.
             */
            StageContributionBase nextStage = CurrentStageContribution.GetNextStageIfStageEnded();
            this.CurrentStageContribution = nextStage;
        }

        // TODO: Might wanna move this above so the caller handles deletion and completion completely. 
        if (this.CurrentStageContribution.IsProductionComplete)
        {
            CompleteProduction(player);
        }
    }

    /// <summary>
    /// Completes the production by creating the finished item and giving it to the player who provided the last TP.
    /// The locked items are consumed, the unfinished item is removed from the room, and the finished item is added to the player's inventory.
    /// </summary>
    /// <param name="player">The player who receives the finished item (the last TP contributor).</param>
    private void CompleteProduction(Player player)
    {
        // Create the finised item
        CommonItemBase finishedItem = CreateFinishedItem(player);
        // I Verified, the item is picked up by the last player who provided the last TP. 
        player.Inventory.Add(finishedItem);

        // TODO: not actually deleted from DB, just cleared. 

        // Delete (unlink the locked items)
        LockedItems.Clear();


        ItemBase item = player.Room.Inventory.Items.First(x => x.ItemType == this.ItemType);
        player.Room.Inventory.Items.Remove(item);

        // Add it to the inventory
    }

    /// <summary>
    /// Unlocks all locked items, returning them to the room's inventory.
    /// Called when a production is cancelled, releasing the items back for other uses.
    /// </summary>
    public void UnlockAllLockedItems()
    {
        foreach (CommonItemBase normalItemBase in this.LockedItems)
        {
            normalItemBase.IsLocked = false;
        }
    }

    /// <summary>
    /// Creates the finished production item to be given to the player upon completion.
    /// </summary>
    /// <param name="player">The player who receives the finished item.</param>
    /// <returns>The finished item created from this production.</returns>
    protected abstract CommonItemBase CreateFinishedItem(Player player);

    /// <summary>
    /// Converts this production item to an ActionTarget for use in action selection UI.
    /// </summary>
    /// <returns>An ActionTarget representing this unfinished production item.</returns>
    public ActionTarget ToActionTarget()
    {
        ActionTarget actionTarget = new ActionTarget()
        {
            Name = this.ItemType.ToString(),
            TargetId = this.Id
        };

        return actionTarget;
    }
}