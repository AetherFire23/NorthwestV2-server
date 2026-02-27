namespace NorthwestV2.Practical.Repositories;

public class WarehouseRepository
{
    private readonly NorthwestContext _northwestContext;

    public WarehouseRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }
}