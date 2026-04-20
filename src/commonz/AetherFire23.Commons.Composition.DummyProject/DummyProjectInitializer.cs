using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.Commons.Composition.DummyProject;

public class DummyProjectInitializer : IInitializer
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<InstalledAndInitializedClass>();
    }

    public void Initialize(IServiceProvider serviceProvider)
    {
        var sp = serviceProvider.GetRequiredService<InstalledAndInitializedClass>();

        sp.ImAnInitializer("Terrible homme mourir faim mort");
    }
}