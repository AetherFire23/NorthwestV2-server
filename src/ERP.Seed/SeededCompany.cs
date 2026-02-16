using AetherFire23.Commons.Seeding;
using Mediator;
using NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;

namespace ERP.Seed;

public class SeededCompany : ISeeder
{
    private readonly IMediator _mediator;

    public SeededCompany(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SetupSeeding()
    {
        await _mediator.Send(new CreateCompanyRequest
        {
            AdminUserName = "fred",
            CompanyName = "FredCo",
            Password = "BONJOUR"
        });
    }
}