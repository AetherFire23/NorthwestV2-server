using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

public class ProductionContributionCost
{
    public int Value => _value;
    private int _value;

    public Roles RoleBonus;
    public string ActionName { get; set; }

    public ProductionContributionCost(int value, string actionName)
    {
        _value = value;
        ActionName = actionName;
    }
}