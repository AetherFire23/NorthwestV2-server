using AetherFire23.Commons.Seeding;
using Microsoft.Extensions.DependencyInjection;

namespace AetheriFire23.Commons.Seeding.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GivenValidArgs_WhenSeedExecuted_ThenDoesNotThrow()
    {
        var sc = new ServiceCollection();

        sc.AddSeedServices(typeof(Tests).Assembly);

        var sp = sc.BuildServiceProvider();

        TestDelegate del = () => sp.ExecuteSeedFromArgs(["--scenario TestSeeder"]);

        Assert.DoesNotThrow(del, "Execute a seed from the arguments should not throw.");
    }

    [Test]
    public void GivenValidCompanyName_WhenSeedExecuted_ThenDoesNotThrow()
    {
        var sc = new ServiceCollection();

        sc.AddLogging();

        sc.AddSeedServices(typeof(Tests).Assembly);

        var sp = sc.BuildServiceProvider();

        TestDelegate del = () => sp.ExecuteSeedFromSeedName("TestSeeder");

        Assert.DoesNotThrow(del, "Execute a seed from the arguments should not throw.");
    }
}