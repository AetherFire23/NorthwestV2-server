using AetherFire23.ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Practical;

namespace NorthwestV2.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NorthwestContext _northwestContext;

    public UserRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public async Task<User> GetById(Guid id)
    {
        User user = await _northwestContext.Users.FindById(id);
        return user;
    }

    public async Task<User> GetByUserName(string username)
    {
        User user = await _northwestContext.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user is null)
        {
            throw new Exception($"User was not found{username}");
        }

        return user;
    }

    public async Task<IEnumerable<User>> GetAllById(List<Guid> userIds)
    {
        List<User> users = new List<User>();

        foreach (Guid userId in userIds)
        {
            User user = await _northwestContext.Users.FindById(userId);

            users.Add(user);
        }

        return users;
    }

    public void Add(User user)
    {
        _northwestContext.Users.Add(user);
    }

    public void AddRange(IEnumerable<User> user)
    {
        foreach (User user1 in user)
        {
            _northwestContext.Users.Add(user1);
        }
    }
}