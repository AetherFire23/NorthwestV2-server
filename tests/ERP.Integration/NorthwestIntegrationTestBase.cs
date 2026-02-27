using System.Reflection;
using AetherFire23.Commons.Testing;
using AetherFire23.ERP.Domain;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthwestV2.Application.Installation;
using NorthwestV2.Infrastructure.Contexts;
using NorthwestV2.Practical;
using Xunit.Abstractions;

namespace NorthwestV2.Integration;

public class NorthwestIntegrationTestBase : PostgresTestContainer
{
    protected readonly IMediator Mediator;
    protected readonly NorthwestContext Context;

    public NorthwestIntegrationTestBase(ITestOutputHelper output) : base(output)
    {
        Mediator = GetService<IMediator>();
        Context = GetService<NorthwestContext>();
    }

    protected override IEnumerable<Assembly> ProvideInstallerAssemblies()
    {
        return
        [
            typeof(DomainInstaller).Assembly,
            typeof(NorthwestContext).Assembly,
            typeof(NorthwestContextInstaller).Assembly,
            typeof(ApplicationInstaller).Assembly,
            typeof(NorthwestIntegrationTestBase).Assembly,
        ];
    }

    protected override void ConfigureAdditionalServices(IServiceCollection serviceCollection)
    {
        /*
         * Logging is outputted to every log outputter. ILoggerProvider.
         */
        serviceCollection.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddProvider(new XUnitLoggerProvider(Output));
            builder.SetMinimumLevel(LogLevel.Debug);
        });

       
    }
    
    // TODO: Configure a InScope deleagate 
    
    public void Dispose()
    {
    }
}