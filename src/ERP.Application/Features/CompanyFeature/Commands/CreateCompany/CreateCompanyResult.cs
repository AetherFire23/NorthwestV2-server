namespace NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;

public class CreateCompanyResult
{
    public required Guid CompanyId { get; set; }
    public required Guid UserId { get; set; }
}