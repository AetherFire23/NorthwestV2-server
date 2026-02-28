using System.Data.Entity;
using AetherFire23.Commons.Domain.Entities;

namespace NorthwestV2.Integration.Extensions;

public static class EfCoreContextExtensions
{
    public static async Task<T> FindOrThrow<T>(this DbContext context, Guid id) where T : EntityBase
    {
        T entity = await context.Set<T>().FindAsync(id);

        if (entity is null)
        {
            throw new Exception($"Entity not found with id{id} of type {typeof(T)} ");
        }

        return entity;
    }
}