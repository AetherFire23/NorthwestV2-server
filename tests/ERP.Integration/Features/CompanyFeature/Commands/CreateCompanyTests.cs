using NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;
using Xunit.Abstractions;

namespace ERP.Integration.Features.CompanyFeature.Commands;

public class CreateCompanyTests : ErpIntegrationTestBase
{
    public CreateCompanyTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenUser_CreatesACompany_ThenTheCompanyExists()
    {
        await Mediator.Send(new CreateCompanyRequest
        {
            CompanyName = "FredCo",
            AdminUserName = "admin",
            Password = "BONJOUR"
        });

        Assert.NotEmpty(base.Context.CompanyInfo);
    }

    [Fact]
    public async Task GivenUser_CreatesACompany_ThenTheUserExists()
    {
        await Mediator.Send(new CreateCompanyRequest
        {
            CompanyName = "FredCo",
            AdminUserName = "admin",
            Password = "Bonjour",
        });

        Assert.NotEmpty(base.Context.Users);
    }

    [Fact]
    public async Task GivenTwoCompanies_WhenAdded_ThenExceptionIsThrown()
    {
        await Mediator.Send(new CreateCompanyRequest
        {
            CompanyName = "FredCo",
            AdminUserName = "admin",
            Password = "Bonjour",
        });

        Action action = () => Mediator.Send(new CreateCompanyRequest
        {
            CompanyName = "FredCo",
            AdminUserName = "admin",
            Password = "Bonjour",
        }).AsTask().Wait();

        Assert.ThrowsAny<Exception>(action);
    }
}