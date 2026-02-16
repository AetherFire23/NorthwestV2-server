using AetherFire23.Commons.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;

namespace NorthwestV2.Application.Installation;

public class MediatorInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMediator(o =>
        {
            o.Assemblies = [typeof(MediatorInstaller).Assembly];
            o.PipelineBehaviors = [typeof(CreateCompanyValidation)];

            /* VERY IMPORTANT that this is added as scoped.
             Will work in test assemblies but not in aspnet core
             */

            o.ServiceLifetime = ServiceLifetime.Scoped;
        });
    }
}