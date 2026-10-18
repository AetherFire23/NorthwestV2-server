using NorthwestV2.ApplicationsStuff.EfCoreExtensions;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Repositories;

public class ItemRepository : IItemrepository
{
    private NorthwestContext _context;

    public ItemRepository(NorthwestContext context)
    {
        _context = context;
    }

    public async Task<ItemBase> Find(Guid itemId)
    {
        ItemBase itemBase = await _context.Items.FindById(itemId);
        return itemBase;
    }
}