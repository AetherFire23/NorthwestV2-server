using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;

public class CreateCompanyValidation : IPipelineBehavior<CreateCompanyRequest, CreateCompanyResult>
{
    private readonly ErpContext _erpContext;

    public CreateCompanyValidation(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }

    public async ValueTask<CreateCompanyResult> Handle(CreateCompanyRequest message,
        MessageHandlerDelegate<CreateCompanyRequest, CreateCompanyResult> next, CancellationToken cancellationToken)
    {
        var company = await _erpContext.CompanyInfo.ToListAsync(cancellationToken);

        const int ONLY_ONE_COMPANY = 1;
        if (company.Count == ONLY_ONE_COMPANY)
        {
            throw new MultipleCompaniesCreatedException();
        }

        return await next(message, cancellationToken);
    }
}