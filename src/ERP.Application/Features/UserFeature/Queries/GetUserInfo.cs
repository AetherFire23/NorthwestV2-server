using Mediator;
using NorthwestV2.Practical;
using System.Data.Entity;

namespace NorthwestV2.Application.Features.UserFeature.Queries;

public class GetUserInfoHandler : IRequestHandler<GetUserInfoRequest>
{
    private readonly ErpContext _erpContext;

    public GetUserInfoHandler(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }

    public async ValueTask<Unit> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
    {
        var user = await _erpContext.Users.FirstAsync(x =>
            x.Username == request.Username && request.Password == x.HashedPassword);

        return Unit.Value;
    }
}