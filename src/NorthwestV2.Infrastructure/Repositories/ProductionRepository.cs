using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.Repositories;

namespace NorthwestV2.Infrastructure.Repositories;

public class ProductionRepository : IProductionRepository
{
    private readonly NorthwestContext _northwestContext;

    public ProductionRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }


}