using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.Commons.Composition.DummyProject;

public class InstalledAndUninstallezInitializer : IInitializer
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<MyOtherService>();
    }

    public void Initialize(IServiceProvider serviceProvider)
    {
    }
}