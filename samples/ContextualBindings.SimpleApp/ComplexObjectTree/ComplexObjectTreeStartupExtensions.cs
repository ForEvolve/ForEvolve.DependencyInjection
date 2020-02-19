using ContextualBindings.SimpleApp.ComplexObjectTree;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ComplexObjectTreeStartupExtensions
    {
        public static IServiceCollection AddComplexObjectTree(this IServiceCollection services)
        {
            services
                .AddContextualBinding<IComplexObjectTreeService, ComplexObjectTreeService>(setup: csd =>
                {
                    csd.WithConstructorArgument<IDirectDependency, DirectDependency1>(setup: csd =>
                    {
                        csd.WithConstructorArgument<ISubDependency1, SubDependency1_1>();
                        csd.WithConstructorArgument<ISubDependency2, SubDependency2_1>();
                        csd.WithConstructorArgument<ISubDependency3, SubDependency3_1>();
                    });
                    csd.WithConstructorArgument<IDirectDependency, DirectDependency2>(setup: csd =>
                    {
                        csd.WithConstructorArgument<ISubDependency1, SubDependency1_2>();
                        csd.WithConstructorArgument<ISubDependency2, SubDependency2_2>();
                        csd.WithConstructorArgument<ISubDependency3, SubDependency3_2>();
                    });
                });
            return services;
        }
    }
}
