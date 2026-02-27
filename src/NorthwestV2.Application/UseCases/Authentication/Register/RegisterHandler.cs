using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.Authentication.Register;

public class RegisterHandler : IRequestHandler<RegisterRequest, Guid>
{
    private readonly NorthwestContext _northwestContext;

    public RegisterHandler(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> THe user id </returns>
    public async ValueTask<Guid> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var (username, password) = request;

        User user = new User()
        {
            Username = username,
            HashedPassword = password // TODO: Has lol 
        };

        _northwestContext.Users.Add(user);

        await _northwestContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}