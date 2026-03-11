namespace NorthwestV2.Application;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}