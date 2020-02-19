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
            // Inject a LastNameGenerator instance into the INameGenerator parameter of FirstNameService
            services
                .AddContextualBinding<IFirstNameService, FirstNameService>(setup: ctx => ctx.WithConstructorArgument<INameGenerator, FirstNameGenerator>())
            ;
            // Inject a LastNameGenerator instance into the INameGenerator parameter of LastNameService
            services
                .AddContextualBinding<ILastNameService, LastNameService>(setup: ctx => ctx
                .WithConstructorArgument<INameGenerator, LastNameGenerator>())
            ;
            // Inject a FirstNameGenerator in the first INameGenerator parameter and
            // inject a LastNameGenerator in the second INameGenerator parameter of FullNameGenerator.
            services
                .AddContextualBinding<INameGenerator, FullNameGenerator>(setup: ctx => ctx
                .WithConstructorArgument<INameGenerator, FirstNameGenerator>()
                .WithConstructorArgument<INameGenerator, LastNameGenerator>())
            ;
            return services;
        }
    }
}
