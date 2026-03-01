using JetBrains.Annotations;
using NorthwestV2.Integration;

namespace ERP.Testing.Domain;

[TestSubject(typeof(FakeRandom))]
public class FakeRandomTest
{
    [Fact]
    public void GiveFakeRandom_WhenNumberFetched_IsCorrectNumber()
    {
        FakeRandom fakeRandom = new FakeRandom([1, 2, 3, 4, 5]);

        int fk = fakeRandom.Next(99, 99);

        Assert.Equal(1, fk);
    }

    [Fact]
    public void GiveFakeRandom_WhenNumberFetchedManyTimes_IsCorrectNumber()
    {
        FakeRandom fakeRandom = new FakeRandom([1, 2, 3, 4, 5]);

        int fk = fakeRandom.Next(99, 99);
        int fk2 = fakeRandom.Next(99, 99);

        Assert.Equal(2, fk2);
    }

    [Fact]
    public void GivenShuffleList_WhenShuffled_IsShuffled()
    {
        FakeRandom fakeRandom = new FakeRandom([1, 0]);

        List<int> integers = new List<int>()
        {
            1, 2
        };

        IEnumerable<int> shuffled = fakeRandom.Shuffle(integers);

        // 1 and 0 are now swapped 
        Assert.Equal(2, shuffled.ElementAt(0));
    }
}