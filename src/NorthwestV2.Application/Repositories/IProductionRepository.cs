using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

namespace NorthwestV2.Application.Repositories;

public interface IProductionRepository
{
    public Task<Production> GetProduction(Guid productionId);
}