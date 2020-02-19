using ContextualBindings.SimpleApp.ContextualControllerInjection;
using ForEvolve.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ContextualControllerInjectionModule : DependencyInjectionModule
    {
        public ContextualControllerInjectionModule(IServiceCollection services)
            : base(services)
        {
            services
                .AddContextualBinding<FirstController>(setup: ctx => ctx.WithConstructorArgument<IService, Implementation1>())
                .AddContextualBinding<SecondController>(setup: ctx => ctx.WithConstructorArgument<IService, Implementation2>())
            ;
        }
    }
}
