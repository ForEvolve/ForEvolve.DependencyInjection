using ContextualBindings.SimpleApp.ComplexObjectTree;
using ForEvolve.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ComplexObjectTreeModule : DependencyInjectionModule
    {
        public ComplexObjectTreeModule(IServiceCollection services)
            : base(services)
        {
            services
                .AddContextualBinding<IComplexObjectTreeService, ComplexObjectTreeService>(d =>
                {
                    d.WithConstructorArgument<IDirectDependency, DirectDependency1>(d => d
                        .WithConstructorArgument<ISubDependency1, SubDependency1_1>()
                        .WithConstructorArgument<ISubDependency2, SubDependency2_1>()
                        .WithConstructorArgument<ISubDependency3, SubDependency3_1>()
                    );
                    d.WithConstructorArgument<IDirectDependency, DirectDependency2>(d => d
                        .WithConstructorArgument<ISubDependency1, SubDependency1_2>()
                        .WithConstructorArgument<ISubDependency2, SubDependency2_2>()
                        .WithConstructorArgument<ISubDependency3, SubDependency3_2>()
                    );
                })
            ;
        }
    }
}
