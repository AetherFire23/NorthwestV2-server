using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.Commons.Composition;

public interface IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration);
}