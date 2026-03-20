using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

public class Production : EntityBase
{
    public  List<Item> LockedItems { get; set; }
}

public class ProdStage
{
    
}