using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.ApplicationsStuff.Repositories;

namespace NorthwestV2.Infrastructure.Repositories;

public class ProductionRepository : IProductionRepository
{
    private readonly NorthwestContext _northwestContext;

    public ProductionRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }


}