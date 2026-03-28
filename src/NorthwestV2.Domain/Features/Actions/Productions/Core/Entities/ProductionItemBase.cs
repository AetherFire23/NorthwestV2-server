using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

/// <summary>
/// Unfinished items behave like normal items, they can be picked up, carried away, and left in other rooms.
/// They have the accumulated Time points.
/// Programmatically: they are unfinished items and contain at least a stage. 
/// </summary>
public class ProductionItemBase : ItemBase
{
    // TODO: Include a Stage 
    public List<ItemBase> LockedItems { get; set; }

    public StageBase CurrentStage { get; set; }
    // TODO: Maybe move 

    public ProductionItemBase()
    {
    }

    public ProductionItemBase(ItemTypes itemType, int carryValue, StageBase initialStage) : base(itemType, carryValue)
    {
        this.CurrentStage = initialStage;
    }

    public void LockForProduction(NormalItemBase other)
    {
        if (other.IsLocked)
        {
            throw new Exception($"Cannot be locked twice - {other}");
        }

        other.IsLocked = true;
        other.ProductionItem = this;
    }

    public void Contribute(Player player)
    {
        if (player.ActionPoints - 1 == -1)
        {
            throw new Exception("Player does not have enough points to contribute.");
        }

        player.ActionPoints--;

        // Increment the current stage by 1 
        this.CurrentStage = this.CurrentStage with { Contributions = this.CurrentStage.Contributions + 1 };


        if (this.CurrentStage.IsStageEnded && !CurrentStage.IsProductionComplete)
        {
            var nextStage = CurrentStage.GetNextStageIfStageEnded();
            this.CurrentStage = nextStage;
        }
    }
}