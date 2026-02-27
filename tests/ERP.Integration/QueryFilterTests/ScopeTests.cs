using Mediator;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Practical;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.QueryFilterTests;

public class ScopeTests : NorthwestIntegrationTestBase
{
    // TODO: Given nwintegrationBase, When creating a new Scope, then requestService exists

    public ScopeTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenRequestContext_WhenCreatedWithNorthwestIntegrationBase_ThenRequestContextServiceExists()
    {
        var srv = GetService<RequestContextService>();

        IEnumerable<Type> serviceTypes = _serviceCollection
            .Select(x => x.ServiceType);
        Assert.Contains(typeof(RequestContextService), serviceTypes);
    }

    [Fact]
    public void GivenRequestContext_WhenCreatedWithScopeNorthwestIntegrationBase_ThenRequestContextServiceExists()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        var requestContextService = scope.ServiceProvider.GetRequiredService<RequestContextService>();
    }

    [Fact]
    public async Task GivenNorthwestContext_WhenQueryPlayer_ThenProvidesUserId()
    {
        // Step 1 : Register
        using (var scope1 = _serviceProvider.CreateScope())
        {
            RequestContextService requestContextService = scope1.ServiceProvider.GetRequiredService<RequestContextService>();
            var mediatorScoped = scope1.ServiceProvider.GetRequiredService<IMediator>();
            await mediatorScoped.Send(new RegisterRequest()
            {
                Username = "Fred",
                Password = "123"
            });
            
        }
        // Step 2 : Login again
        using (var scope1 = _serviceProvider.CreateScope())
        {
            RequestContextService requestContextService = scope1.ServiceProvider.GetRequiredService<RequestContextService>();
            var mediatorScoped = scope1.ServiceProvider.GetRequiredService<IMediator>();
            await mediatorScoped.Send(new LoginRequest()
            {
                Username = "Fred",
                Password = "123"
            });
        }
        // Step 3 : Do some action while logged in 
        
        using (var scope1 = _serviceProvider.CreateScope())
        {
         // TODO: Action while logged in (i.e.) creating a lobby. 
        }
      



        // NorthwestContext anyService = scope.ServiceProvider.GetRequiredService<NorthwestContext>();
    }
}