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
            // Create default bindings
            services.AddSingleton<ISubDependency1, SubDependency1_1>();
            services.AddSingleton<ISubDependency2, SubDependency2_1>();
            services.AddSingleton<ISubDependency3, SubDependency3_1>();

            // Contextual Injection
            services
                .AddContextualBinding<IComplexObjectTreeService, ComplexObjectTreeService>()
                .WithConstructorArgument<IDirectDependency, DirectDependency1>()
                .WithConstructorArgument<IDirectDependency, DirectDependency2>()
            ;

            // Allow to build a subtree within the ComplexObjectTreeService binding
            services
                .AddContextualBinding<IDirectDependency, DirectDependency2>()
                .WithConstructorArgument<ISubDependency1, SubDependency1_2>()
                .WithConstructorArgument<ISubDependency2, SubDependency2_2>()
                .WithConstructorArgument<ISubDependency3, SubDependency3_2>()
            ;
            return services;
        }
    }
}
