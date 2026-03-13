using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Application.Features.Actions.General.Combat;
using NorthwestV2.Application.Repositories;

namespace NorthwestV2.Application.UseCases.Authentication.Login;

/// <summary>
/// Handles a <see cref="LoginRequest"/> by validating user credentials and
/// returning a corresponding <see cref="LoginResult"/> upon successful authentication.
/// </summary>
/// <remarks>
/// This handler performs a simple username lookup and compares the provided password
/// against the stored value. The current implementation uses plain-text comparison
/// and should be replaced with proper password hashing and verification.
/// </remarks>
public class LoginHandler : IRequestHandler<LoginRequest, LoginResult>
{
    private readonly IUserRepository _userRepository;

    public LoginHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<LoginResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByUserName(request.Username);

        // TODO: Hash and check passwords for real.

        if (user.HashedPassword != request.Password)
        {
            throw new LoginException();
        }

        return new LoginResult
        {
            UserId = user.Id,
        };
    }
}