using ContextualBindings.SimpleApp.ContextualServiceInjection;
using ForEvolve.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ContextualServiceInjectionModule : DependencyInjectionModule
    {
        public ContextualServiceInjectionModule(IServiceCollection services)
            : base(services)
        {
            // Inject a LastNameGenerator instance into the INameGenerator parameter of FirstNameService
            services
                .AddContextualBinding<IFirstNameService, FirstNameService>(d => d
                    .WithConstructorArgument<INameGenerator, FirstNameGenerator>())
            ;
            // Inject a LastNameGenerator instance into the INameGenerator parameter of LastNameService
            services
                .AddContextualBinding<ILastNameService, LastNameService>(d => d
                    .WithConstructorArgument<INameGenerator, LastNameGenerator>())
            ;
            // Inject a FirstNameGenerator in the first INameGenerator parameter and
            // inject a LastNameGenerator in the second INameGenerator parameter of FullNameGenerator.
            services
                .AddContextualBinding<INameGenerator, FullNameGenerator>(d => d
                    .WithConstructorArgument<INameGenerator, FirstNameGenerator>()
                    .WithConstructorArgument<INameGenerator, LastNameGenerator>())
            ;
        }
    }
}
