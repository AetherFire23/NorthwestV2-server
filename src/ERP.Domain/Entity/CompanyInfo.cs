using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class CompanyInfo : EntityBase
{
    public required string CompanyName { get; set; }

    public virtual ICollection<Warehouse> Warehouses { get; set; } = [];

    private CompanyInfo()
    {
    }

    public static CompanyInfo Create(string name)
    {
        CompanyInfo companyInfo = new()
        {
            CompanyName = name
        };

        return companyInfo;
    }
}