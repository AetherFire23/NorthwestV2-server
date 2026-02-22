namespace NorthwestV2.Practical.Repositories;

public class WarehouseRepository
{
    private readonly ErpContext _erpContext;

    public WarehouseRepository(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }
}