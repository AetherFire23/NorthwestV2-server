using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using JetBrains.Annotations;

namespace ERP.Testing.Domain.Actions.AvailabilityStuff;

[TestSubject(typeof(ActionTarget))]
public class ActionTargetTest
{
    private static ActionTarget ANY_ACTION_TARGET_ID_1 = new ActionTarget()
    {
        TargetId = new Guid("aca461cc-f305-4fad-881b-1f4f488ebc41"),
    };
    
    private static ActionTarget OTHER_INSTANCE_WITH_SAME_ID = new ActionTarget()
    {
        TargetId = new Guid("aca461cc-f305-4fad-881b-1f4f488ebc41"),
    };

    private static ActionTarget ANY_ACTION_TARGET_ID_2 = new ActionTarget()
    {
        TargetId = new Guid("c78c74f9-5af8-4fea-abca-f5807d186d15"),
    };

    private static ActionTarget ANY_ACTION_TARGET_VALUE_1 = new ActionTarget()
    {
        Value = "1",
    };
    
    private static ActionTarget OTHER_ACTION_TARGET_VALUE_1 = new ActionTarget()
    {
        Value = "1",
    };

    private static ActionTarget ANY_ACTION_TARGET_VALUE_2 = new ActionTarget()
    {
        Value = "2",
    };
    
    private static ActionTarget BOTH_ID_AND_VALUE_DEFINED = new ActionTarget()
    {
        Value = "2",
        TargetId = new Guid("6c3114b9-841f-4a84-a371-c048377c1105") 
    };

    [Fact]
    public void GivenTwoSameIds_WhenEquals_ThenAreTheSame()
    {
        bool result = ANY_ACTION_TARGET_ID_1.Equals(OTHER_INSTANCE_WITH_SAME_ID);
        Assert.True(result);
    }
    
    [Fact]
    public void GivenTwoSameValues_WhenEquals_ThenAreTheSame()
    {
        bool result = ANY_ACTION_TARGET_VALUE_1.Equals(OTHER_ACTION_TARGET_VALUE_1);
        Assert.True(result);
    }
    
    [Fact]
    public void GivenTwoDifferentIds_WhenEquals_ThenAreNotEquals()
    {
        bool result = ANY_ACTION_TARGET_ID_1.Equals(ANY_ACTION_TARGET_ID_2);
        Assert.False(result);
    }
    
        
    [Fact]
    public void GivenTwoDifferentValues_WhenEquals_ThenAreNotEquals()
    {
        bool result = ANY_ACTION_TARGET_VALUE_1.Equals(ANY_ACTION_TARGET_VALUE_2);
        Assert.False(result);
    }
    
    [Fact]
    public void GivenIdAndBothDefined_WhenEquals_ThenThrowsException()
    {
        var idOnly = ANY_ACTION_TARGET_ID_1;

        Assert.Throws<Exception>(() => idOnly.Equals(BOTH_ID_AND_VALUE_DEFINED));
    }
}