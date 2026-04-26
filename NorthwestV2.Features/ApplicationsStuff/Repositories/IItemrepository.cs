using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.ApplicationsStuff.Repositories;

public interface IItemrepository
{
    public Task<ItemBase> Find(Guid itemId);
}