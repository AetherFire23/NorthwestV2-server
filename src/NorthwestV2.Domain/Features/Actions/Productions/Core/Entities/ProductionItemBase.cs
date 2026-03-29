using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

/// <summary>
/// Unfinished items behave like normal items, they can be picked up, carried away, and left in other rooms.
/// They have the accumulated Time points.
/// Programmatically: they are unfinished items and contain at least a stage. 
/// </summary>
public abstract class ProductionItemBase : ItemBase
{
    // TODO: Include a Stage 
    public List<ItemBase> LockedItems { get; set; } = [];

    public StageContributionBase CurrentStageContribution { get; set; }
    // TODO: Maybe move 

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
        if (other.IsLocked)
        {
            throw new Exception($"Cannot be locked twice - {other}");
        }

        other.IsLocked = true;
        other.Inventory = null;

        this.LockedItems.Add(other);
    }

    public void Contribute(Player player)
    {
        bool wouldHitPointsFallBelowZero = player.ActionPoints - 1 == -1;
        if (wouldHitPointsFallBelowZero)
        {
            throw new Exception("Player does not have enough points to contribute.");
        }

        player.ActionPoints--;

        // Increment the current stage by 1 
        this.CurrentStageContribution = this.CurrentStageContribution with
        {
            Contributions = this.CurrentStageContribution.Contributions + 1
        };

        bool isCurrentStadedEndedButCurrentIsNotComplete =
            this.CurrentStageContribution.IsStageEnded && !CurrentStageContribution.IsProductionComplete;
        if (isCurrentStadedEndedButCurrentIsNotComplete)
        {
            var nextStage = CurrentStageContribution.GetNextStageIfStageEnded();
            this.CurrentStageContribution = nextStage;
        }


        if (this.CurrentStageContribution.IsProductionComplete)
        {
            OnProductionCompleted(player);
        }
    }

    public abstract void OnProductionCompleted(Player player);
}