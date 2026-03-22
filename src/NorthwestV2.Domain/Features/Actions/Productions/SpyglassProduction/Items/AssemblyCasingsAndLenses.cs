using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

/// <summary>
/// Intermediate item
/// </summary>
public class AssemblyCasingsAndLenses : ItemBase
{
    public const int CARRY_VALUE = 1;

    public AssemblyCasingsAndLenses() : base(ItemTypes.AssemblyOfCasingAndLenses, CARRY_VALUE)
    {
        
    }
}
