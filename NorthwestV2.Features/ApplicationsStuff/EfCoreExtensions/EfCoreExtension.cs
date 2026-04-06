using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.EfCoreExtensions;

/// <summary>
/// HAve to use Context.Set<T>().FindById in order for this to work.
/// </summary>
public static class EfCoreExtension
{
    public static async Task<T> FindById<T>(this DbSet<T> curr, Guid id)
        where T : EntityBase
    {
        var ent = await curr.FindAsync(id);

        if (ent is null)
        {
            throw new Exception($"Entity {typeof(T)} with id {id} does not exist.");
        }

        return ent;
    }

    public static async Task<IEnumerable<T>> FindAllById<T>(this DbSet<T> curr, IEnumerable<Guid> ids)
        where T : EntityBase
    {
        List<T> entities = new List<T>();

        foreach (Guid guid in ids)
        {
            T entity = await curr.FindById(guid);
            entities.Add(entity);
        }

        return entities;
    }
}