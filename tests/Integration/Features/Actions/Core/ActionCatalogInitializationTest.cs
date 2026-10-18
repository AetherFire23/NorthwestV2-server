using JetBrains.Annotations;
using NorthwestV2.Features.Features.Actions.Core.Application;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Core;

[TestSubject(typeof(ActionCatalog))]
public class ActionCatalogInitializationTest : NorthwestTestBase
{
    public ActionCatalogInitializationTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenInitializedServices_WhenReadingNameMap_ThenEveryActionIsResolvableByName()
    {
        ActionCatalog catalog = GetServiceFromScope<ActionCatalog>();

        Assert.Equal(catalog.AllActionTypes.Count, catalog.ActionTypesByName.Count);
    }

    [Fact]
    public async Task GivenUnknownActionName_WhenGettingActionFromName_ThenThrows()
    {
        ActionServices actionServices = GetServiceFromScope<ActionServices>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => actionServices.GetActionFromName("no such action"));
    }

    [Fact]
    public async Task GivenKnownActionName_WhenGettingActionFromName_ThenOnlyThatActionIsReturned()
    {
        ActionServices actionServices = GetServiceFromScope<ActionServices>();
        ActionCatalog catalog = GetServiceFromScope<ActionCatalog>();
        string anyName = catalog.ActionTypesByName.Keys.First();

        var action = await actionServices.GetActionFromName(anyName);

        Assert.Equal(anyName, action.ActionName);
    }
}
