using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.Productions.HammerProduction.Contribution;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.HammerProduction.Contribution;

[TestSubject(typeof(HammerProductionContributionApp))]
public class HammerProductionContributionAppTest : NorthwestIntegrationTestBase
{
    public HammerProductionContributionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenWorkshopAndScrap_WhenContributingEnoughTimes_ThenHammerIsInRoomsInventory()
    {
        
        // Assert.True(roomItems.Any(x => x.ItemType == ItemTypes.Hammer));
    }
}