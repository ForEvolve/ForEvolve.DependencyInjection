using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.DependencyInjection
{
    /// <summary>
    /// Inherit from this class as a starting point to create a DI module.
    /// This is just a convenience class, you can also implement
    /// <see cref="IDependencyInjectionModule"/>.
    /// </summary>
    /// <remarks>
    /// When inheriting of this class, make sure to register all dependencies
    /// that need to be injected in the constructor other than <see cref="IServiceCollection>"/>
    /// with the <see cref="IScanningContext"/> by calling the 
    /// <see cref="IScanningContext.ConfigureServices(Action{IServiceCollection})"/>
    /// method or any available extension methods.
    /// </remarks>
    public abstract class DependencyInjectionModule : IDependencyInjectionModule
    {
        /// <summary>
        /// Register the module's dependencies with the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register bindings with.</param>
        public DependencyInjectionModule(IServiceCollection services)
        {

        }

        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
