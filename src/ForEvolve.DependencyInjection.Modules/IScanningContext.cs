using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ForEvolve.DependencyInjection
{
    public interface IScanningContext
    {
        /// <summary>
        /// Gets if the <see cref="IScanningContext.Initialize"/> method has been called.
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Registers <see cref="IDependencyInjectionModule"/> dependencies that can be used during instantiation.
        /// These are only loaded in a local <see cref="IServiceCollection"/>.
        ///
        /// This method can be called multiple times.
        /// </summary>
        /// <param name="setup">Register your dependencies here.</param>
        /// <returns>The current <see cref="IScanningContext"/>.</returns>
        IScanningContext ConfigureServices(Action<IServiceCollection> setup);

        /// <summary>
        /// Register a <see cref="IDependencyInjectionModule"/> to be initialized later.
        /// This method can be called multiple times.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns>The current <see cref="IScanningContext"/>.</returns>
        IScanningContext Register(IEnumerable<TypeInfo> modulesTypeInfo);

        /// <summary>
        /// Call this method to trigger the initialization of all registered <see cref="IDependencyInjectionModule"/>.
        /// Use the Register method to register modules.
        /// Use the ConfigureServices method to configure the services required to instanciate the modules.
        ///
        /// This method must be called exactly once.
        /// </summary>
        /// <exception cref="ScanningContextInitializedException"></exception>
        void Initialize();
    }
}
