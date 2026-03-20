using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

public class Production : EntityBase
{
    public required List<Item> LockedItems { get; set; }
    public required int _currentStageIndex { get; set; }
}

public class ProdStage
{
    
}