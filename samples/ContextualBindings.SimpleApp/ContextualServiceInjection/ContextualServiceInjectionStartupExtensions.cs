using ContextualBindings.SimpleApp.ContextualServiceInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContextualServiceInjectionStartupExtensions
    {
        public static IServiceCollection AddContextualServiceInjection(this IServiceCollection services)
        {
            // Inject a LastNameGenerator instance into the INameGenerator parameter
            services
                .AddContextualBinding<IFirstNameService, FirstNameService>()
                .WithConstructorArgument<INameGenerator, FirstNameGenerator>()
            ;
            // Inject a LastNameGenerator instance into the INameGenerator parameter
            services
                .AddContextualBinding<ILastNameService, LastNameService>()
                .WithConstructorArgument<INameGenerator, LastNameGenerator>()
            ;
            // Inject a FirstNameGenerator in the first INameGenerator parameter and
            // inject a LastNameGenerator in the second INameGenerator parameter.
            services
                .AddContextualBinding<INameGenerator, FullNameGenerator>()
                .WithConstructorArgument<INameGenerator, FirstNameGenerator>()
                .WithConstructorArgument<INameGenerator, LastNameGenerator>()
            ;
            return services;
        }
    }
}
