using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Application.Repositories;

namespace NorthwestV2.Application.UseCases.Authentication.Register;

public class RegisterHandler : IRequestHandler<RegisterRequest, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
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
        
        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}