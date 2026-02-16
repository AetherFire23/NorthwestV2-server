using AetherFire23.Commons.Testing;
using AetherFire23.ERP.Domain;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthwestV2.Application.Installation;
using NorthwestV2.Infrastructure.Contexts;
using NorthwestV2.Practical;
using System.Reflection;
using Xunit.Abstractions;

namespace ERP.Integration;

public class ErpIntegrationTestBase : PostgresTestContainer
{
    protected IMediator Mediator;
    protected ErpContext Context;

    // TODO: 
    public ErpIntegrationTestBase(ITestOutputHelper output) : base(output)
    {
        Mediator = GetService<IMediator>();
        Context = GetService<ErpContext>();
    }

    protected override IEnumerable<Assembly> ProvideInstallerAssemblies()
    {
        return
        [
            typeof(DomainInstaller).Assembly,
            typeof(ErpContext).Assembly,
            typeof(ErpContextInstaller).Assembly,
            typeof(ApplicationInstaller).Assembly,
            typeof(ErpIntegrationTestBase).Assembly,
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

    public void Dispose()
    {
    }
}