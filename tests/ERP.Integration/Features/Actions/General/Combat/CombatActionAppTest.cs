using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.General.Combat;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Combat;

[TestSubject(typeof(CombatActionApp))]
public class CombatActionAppTest : NorthwestIntegrationTestBase
{
    public CombatActionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenPlayerInRoom_WhenGettingAvailability_ThenOnlyPlayersInSameRoomAreVisible()
    {
        
    }
}