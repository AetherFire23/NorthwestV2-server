using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Api.Composition;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Infrastructure;
using NorthwestV2.Infrastructure.Contexts;
using NorthwestV2.Integration.Tooling;
using Xunit.Abstractions;

namespace NorthwestV2.Integration;

public class NorthwestTestBase : IntegrationTestBase
{
    protected NorthwestTestBase(ITestOutputHelper output) : base(output)
    {
    }

    public IServiceScope Scope
    {
        get => CurrentScope;
        set => CurrentScope = value;
    }

    protected IMediator Mediator => CurrentScope.ServiceProvider.GetRequiredService<IMediator>();
    public NorthwestContext Context => CurrentScope.ServiceProvider.GetRequiredService<NorthwestContext>();
    public IPlayerRepository PlayerRepository => CurrentScope.ServiceProvider.GetRequiredService<IPlayerRepository>();

    public override string ProvideDatabaseName() => NorthwestContextInstaller.ConnectionStringKey;

    public override Task AddRequiredConfigurations(Dictionary<string, string> configurations)
    {
        return Task.CompletedTask;
    }

    public override Task AddServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddCoreServices(configuration);
        OverrideServices(serviceCollection);

        return Task.CompletedTask;
    }

    public override async Task InitializeServices(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        await serviceProvider.InitializeServices(configuration);
    }

    /// <summary>
    /// Replace production registrations (ex: IRandomProvider with FakeRandom) after AddCoreServices.
    /// </summary>
    protected virtual void OverrideServices(IServiceCollection serviceCollection)
    {
    }
}
