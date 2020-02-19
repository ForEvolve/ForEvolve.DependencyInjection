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
                })
            ;
        }
    }
}
