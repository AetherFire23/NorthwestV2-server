using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Practical;
using NorthwestV2.Application.EfCoreExtensions;

namespace NorthwestV2.Application.UseCases.GameStart;

public class CreateGameHandler : IRequestHandler<CreateGameRequest, Guid>
{
    private readonly NorthwestContext _northwestContext;
    private readonly PlayerFactory _playerFactory;

    public CreateGameHandler(NorthwestContext northwestContext, PlayerFactory playerFactory)
    {
        _northwestContext = northwestContext;
        _playerFactory = playerFactory;
    }

    public async ValueTask<Guid> Handle(CreateGameRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await _northwestContext.Set<User>().FindAllById(request.UserIds);

        _playerFactory.CreateFreshPlayersForGame(users.ToList());

        return Guid.Empty;
    }

    private async Task<IEnumerable<User>> GetUsers(List<Guid> userIds)
    {
        List<User> users = new List<User>();

        foreach (Guid userId in userIds)
        {
            User user = await _northwestContext.Users.FindAsync(userId) ?? throw new Exception("Cannot find this user");
            users.Add(user);
        }

        return users;
    }
}