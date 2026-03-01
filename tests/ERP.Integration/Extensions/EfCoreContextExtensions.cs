using System.Data.Entity;
using AetherFire23.Commons.Domain.Entities;

namespace NorthwestV2.Integration.Extensions;

public static class EfCoreContextExtensions
{
    public static async Task<T> FindById<T>(this DbSet<T> ctx, Guid id) where T : EntityBase
    {
        T? entity = await ctx.FindAsync(id);
        
        return entity ?? throw new Exception($"Enttiy should not be empty {id}, {typeof(T)}");
    }
    
    public static async Task Anus<T>(this DbSet<T> ctx) where T : EntityBase
    {
        
    }
}