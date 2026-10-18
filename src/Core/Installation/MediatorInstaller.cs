using Microsoft.Extensions.DependencyInjection;

namespace NorthwestV2.Features.Installation;

public static class MediatorInstaller
{
    public static void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediator(o =>
        {
            o.Assemblies = [typeof(MediatorInstaller).Assembly];

            //o.PipelineBehaviors = [typeof(CreateCompanyValidation)];

            /* VERY IMPORTANT that this is added as scoped.
             Will work in test assemblies but not in aspnet core
             */

            o.ServiceLifetime = ServiceLifetime.Scoped;
        });
    }
}