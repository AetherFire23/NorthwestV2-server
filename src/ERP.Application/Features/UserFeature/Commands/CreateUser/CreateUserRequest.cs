using Mediator;

namespace NorthwestV2.Application.Features.UserFeature.Commands.CreateUser;

public class CreateUserRequest : IRequest
{
    public string Username { get; init; }
}