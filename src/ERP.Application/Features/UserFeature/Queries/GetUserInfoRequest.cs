using Mediator;

namespace NorthwestV2.Application.Features.UserFeature.Queries;

public class GetUserInfoRequest : IRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}