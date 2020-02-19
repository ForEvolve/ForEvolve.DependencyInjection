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
        /// <summary>
        /// Enables contextual contructor injection for controllers.
        /// </summary>
        /// <param name="mvcBuilder">The <see cref="IMvcBuilder"/> that controllers were registered with.</param>
        /// <returns>The <paramref name="mvcBuilder"/>.</returns>
        public static IMvcBuilder WithContextualBindings(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services
                .Decorate<IControllerActivator, ControllerActivator>();
            return mvcBuilder;
        }
    }
}
