using System.Reflection;
using AertherFire23.EntityFramework.DummyModule2.Context2;
using AetherFire23.Commons.Composition.DummyProject;
using AetherFire23.Commons.EntityFramework.DummyConsole.Db;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace AetherFire23.Commons.Testing.DummyTestingProject;

public class BaseDummyTestingProjectClass : PostgresTestContainer
{
    public BaseDummyTestingProjectClass(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        
    }
    protected override IEnumerable<Assembly> ProvideInstallerAssemblies()
    {
        return
        [
            typeof(DummyProjectInitializer).Assembly,
            typeof(BaseDummyTestingProjectClass).Assembly,
            typeof(DummyNpgsql).Assembly,
            typeof(DummyNpgsql2).Assembly
        ];
    }
}