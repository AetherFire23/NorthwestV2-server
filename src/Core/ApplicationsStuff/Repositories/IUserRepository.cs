using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.ApplicationsStuff.Repositories;

public interface IUserRepository
{
    public Task<User> GetById(Guid id);
    public Task<User> GetByUserName(string username);

    public Task<IEnumerable<User>> GetAllById(List<Guid> userIds);

    public void Add(User user);
}