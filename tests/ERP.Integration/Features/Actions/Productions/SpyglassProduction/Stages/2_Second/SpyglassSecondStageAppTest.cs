using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction.Stages._2_Second;

namespace NorthwestV2.Integration.Features.Actions.Productions.SpyglassProduction.Stages._2_Second;

[TestSubject(typeof(SpyglassSecondStageApp))]
public class SpyglassSecondStageAppTest
{
    [Fact]
    public void GivenUnfinishedSpyglassAndPlayerInCorrectRoom_WhenGetActions_ThenCanStartSecondStage()
    {
        // actions.Secondstage.CanExecute = true 
        Assert.True(false);
    }
}