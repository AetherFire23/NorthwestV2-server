using JetBrains.Annotations;
using NorthwestV2.Features.Features.Actions.Core.Application;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;

namespace ERP.Testing.Domain.Features.Actions.Core;

[TestSubject(typeof(ActionCatalog))]
public class ActionCatalogTest
{
    private readonly ActionCatalog _catalog = ActionCatalog.Scan(typeof(ActionCatalog).Assembly);

    [Fact]
    public void GivenCoreAssembly_WhenScanned_ThenOnlyConcreteActionsAreFound()
    {
        Assert.NotEmpty(_catalog.AllActionTypes);
        Assert.All(_catalog.AllActionTypes, t => Assert.False(t.IsAbstract));
    }

    [Fact]
    public void GivenCoreAssembly_WhenScanned_ThenInstantAndTargetsPartitionAllActions()
    {
        Assert.All(_catalog.InstantActionTypes, t => Assert.True(typeof(InstantActionBase).IsAssignableFrom(t)));
        Assert.All(_catalog.ActionWithTargetsTypes,
            t => Assert.True(typeof(ActionWithTargetsBase).IsAssignableFrom(t)));
        Assert.Equal(_catalog.AllActionTypes.Count,
            _catalog.InstantActionTypes.Count + _catalog.ActionWithTargetsTypes.Count);
    }

    [Fact]
    public void GivenNotInitialized_WhenReadingNameMap_ThenThrows()
    {
        Assert.Throws<InvalidOperationException>(() => _catalog.ActionTypesByName);
    }
}
