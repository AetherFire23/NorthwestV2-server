using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

/// <summary>
/// Unfinished items behave like normal items, they can be picked up, carried away, and left in other rooms.
/// They have the accumulated Time points.
/// Programmatically: they are unfinished items and contain at least a stage. 
/// </summary>
public abstract class ProductionItemBase : ItemBase
{
    // TODO: Include a Stage 
    public List<NormalItemBase> LockedItems { get; set; } = [];

    public StageContributionBase CurrentStageContribution { get; set; }
    
    // TODO: Maybe check if we  
    public ProductionItemBase()
    {
    }

    public ProductionItemBase(ItemTypes itemType, int carryValue, StageContributionBase initialStageContribution) :
        base(itemType, carryValue)
    {
        this.CurrentStageContribution = initialStageContribution;
    }

    public void LockForProduction(NormalItemBase other)
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
            Contributions = this.CurrentStageContribution.Contributions + 1
        };

        bool isCurrentStadedEndedButCurrentIsNotComplete =
            this.CurrentStageContribution.IsStageEnded &&
            !CurrentStageContribution.IsProductionComplete;
        if (isCurrentStadedEndedButCurrentIsNotComplete)
        {
            var nextStage = CurrentStageContribution.GetNextStageIfStageEnded();
            this.CurrentStageContribution = nextStage;
        }

        // TODO: Might wanna move this above so the caller handles deletion and completion completely. 
        if (this.CurrentStageContribution.IsProductionComplete)
        {
            OnProductionCompleted(player);
        }
    }

    public void UnlockAllLockedItems()
    {
        foreach (NormalItemBase normalItemBase in this.LockedItems)
        {
            normalItemBase.IsLocked = false;
        }
    }


    public abstract void OnProductionCompleted(Player player);

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