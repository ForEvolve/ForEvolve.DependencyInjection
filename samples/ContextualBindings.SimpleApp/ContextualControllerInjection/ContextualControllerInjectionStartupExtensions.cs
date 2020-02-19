using ContextualBindings.SimpleApp.ContextualControllerInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
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
                .AddContextualBinding<FirstController>(setup: ctx => ctx.WithConstructorArgument<IService, Implementation1>())
                .AddContextualBinding<SecondController>(setup: ctx => ctx.WithConstructorArgument<IService, Implementation2>())
            ;
            return services;
        }
    }
}
