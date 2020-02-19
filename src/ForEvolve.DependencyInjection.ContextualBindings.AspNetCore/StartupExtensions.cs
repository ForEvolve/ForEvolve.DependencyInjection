using ForEvolve.DependencyInjection.ContextualBindings.AspNetCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ForEvolveDependencyInjectionContextualBindingsAspNetCoreStartupExtensions
    {
        public static IMvcBuilder WithContextualBindings(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services
                .Decorate<IControllerActivator, ControllerActivator>();
            return mvcBuilder;
        }
    }
}
