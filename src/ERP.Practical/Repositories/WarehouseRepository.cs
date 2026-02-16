using AetherFire23.ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace NorthwestV2.Practical.Repositories;

public class WarehouseRepository
{
    private readonly ErpContext _erpContext;

    public WarehouseRepository(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }
}