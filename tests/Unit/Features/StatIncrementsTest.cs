using JetBrains.Annotations;
using NorthwestV2.Features.Shared;

namespace NorthwestV2.Unit.Features;

[TestSubject(typeof(StatIncrements))]
public class StatIncrementsTest
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(20, 1)]
    [InlineData(21, 2)]
    [InlineData(80, 4)]
    [InlineData(81, 5)]
    [InlineData(100, 5)]
    public void GivenValue_WhenConvertingToIncrements_ThenEveryTwentyPercentIsOneIncrement(int value, int expected)
    {
        int increment = StatIncrements.From(value);

        Assert.Equal(expected, increment);
    }
}
