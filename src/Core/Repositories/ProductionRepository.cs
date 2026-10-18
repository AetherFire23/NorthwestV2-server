using NorthwestV2.ApplicationsStuff.Repositories;

namespace NorthwestV2.Repositories;

public class ProductionRepository : IProductionRepository
{
    private readonly NorthwestContext _northwestContext;

    public ProductionRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }


}