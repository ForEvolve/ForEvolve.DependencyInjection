using ContextualBindings.SimpleApp.ContextualControllerInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContextualControllerInjectionStartupExtensions
    {
        public static IServiceCollection AddContextualControllerInjection(this IServiceCollection services)
        {
            services
                .AddContextualBinding<FirstController>()
                .WithConstructorArgument<IService, Implementation1>()
            ;
            services
                .AddContextualBinding<SecondController>()
                .WithConstructorArgument<IService, Implementation2>()
            ;
            return services;
        }
    }
}

