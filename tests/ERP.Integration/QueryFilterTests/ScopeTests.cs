using NorthwestV2.Application;
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
    }
    
    [Fact]
    public void GivenRequestContext_WhenCreatedWithScopeNorthwestIntegrationBase_ThenRequestContextServiceExists()
    {
        var srv = GetService<RequestContextService>();
    }
}