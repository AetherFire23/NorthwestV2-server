using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.Extensions.Logging;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;

public class CreateCompanyHandler : IRequestHandler<CreateCompanyRequest, CreateCompanyResult>
{
    private readonly ErpContext _erpContext;
    private readonly ILogger<CreateCompanyRequest> _logger;

    public CreateCompanyHandler(ErpContext erpContext, ILogger<CreateCompanyRequest> logger)
    {
        _erpContext = erpContext;
        _logger = logger;
    }

    public async ValueTask<CreateCompanyResult> Handle(CreateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting CreateCompanyHandler added to database");

        CompanyInfo companyInfo = CompanyInfo.Create(request.CompanyName);


        _erpContext.CompanyInfo.Add(companyInfo);
        await _erpContext.SaveChangesAsync(cancellationToken);

        User user = User.Create(request.AdminUserName, request.Password, companyInfo.Id);

        _erpContext.Users.Add(user);

        await _erpContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Company added to database " + companyInfo.CompanyName);

        return new()
        {
            CompanyId = companyInfo.Id,
            UserId = user.Id,
        };
    }
}