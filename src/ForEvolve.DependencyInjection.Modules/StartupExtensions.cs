﻿using ForEvolve.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ForEvolveDependencyInjectionModulesStartupExtensions
    {
        /// <summary>
        /// Create an <see cref="IScanningContext"/>, scan specified assemblies,
        /// and initialized the modules that were found.
        /// </summary>
        /// <param name="services">The service collection to add dependency to.</param>
        /// <param name="assemblies">The assemblies to scan for modules.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDependencyInjectionModules(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddDependencyInjectionModules(initialize: true, assemblies);
            return services;
        }

        /// <summary>
        /// Create an <see cref="IScanningContext"/>, scan specified assemblies,
        /// and optionaly initialized the modules that were found.
        /// </summary>
        /// <param name="services">The service collection to add dependency to.</param>
        /// <param name="initialize">
        /// When true, call the <see cref="IScanningContext.Initialize"/> method of
        /// the <see cref="IScanningContext"/> after scanning the assemblies.
        /// </param>
        /// <param name="assemblies">The assemblies to scan for modules.</param>
        /// <returns>The created <see cref="IScanningContext"/>.</returns>
        public static IScanningContext AddDependencyInjectionModules(this IServiceCollection services, bool initialize, params Assembly[] assemblies)
        {
            var context = new ScanningContext(services);
            if (assemblies != null)
            {
                context.ScanAssemblies(assemblies);
            }
            if (initialize)
            {
                context.Initialize();
            }
            return context;
        }

        /// <summary>
        /// Register the specified <see cref="IConfiguration"/>, used during modules instantiation.
        /// </summary>
        /// <param name="scanningContext">The scanning context to add <see cref="IConfiguration"/> to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to add.</param>
        /// <returns>The <paramref name="scanningContext"/>.</returns>
        public static IScanningContext UseConfiguration(this IScanningContext scanningContext, IConfiguration configuration)
        {
            return scanningContext
                .ConfigureServices(services => services.TryAddSingleton(configuration))
            ;
        }

        /// <summary>
        /// Scan the specified assemblies and registers all <see cref="IDependencyInjectionModule"/>
        /// implementations that are found with the <see cref="IScanningContext"/>.
        ///
        /// This method can be called multiple times.
        /// </summary>
        /// <param name="scanningContext">The <see cref="IScanningContext"/> to registers modules against.</param>
        /// <param name="assemblies">The assemblies to scan for.</param>
        /// <returns>The <paramref name="scanningContext"/>.</returns>
        public static IScanningContext ScanAssemblies(this IScanningContext scanningContext, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (assembly.IsDynamic)
                {
                    continue; // throw an exception? log an error/warning?
                }
                var modulesTypeInfo = assembly
                    .DefinedTypes
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .KeepOnlyDependencyInjectionModules();
                ;
                scanningContext.Register(modulesTypeInfo);
            }
            return scanningContext;
        }

        /// <summary>
        /// Create a <see cref="IScanningContext"/> ready to scan for DI modules.
        /// </summary>
        /// <param name="services">The service collection to add dependency to.</param>
        /// <returns>The newly created <see cref="IScanningContext"/>.</returns>
        [Obsolete("Use AddDependencyInjectionModules(...) instead. This should be removed in v3.0.", false)]
        public static IScanningContext ScanForDIModules(this IServiceCollection services)
        {
            return services.AddDependencyInjectionModules(initialize: false);
        }

        /// <summary>
        /// Scan the assembly containing <typeparamref name="T"/>
        /// and initialize all DI modules found, registering dependencies
        /// with the <see cref="IServiceCollection"/> specified during the
        /// <see cref="IScanningContext"/> creation.
        /// </summary>
        /// <typeparam name="T">The type in an assembly to scan.</typeparam>
        /// <param name="scanningContext">The <see cref="IScanningContext"/> to initialize.</param>
        /// <returns>The <paramref name="scanningContext"/>.</returns>
        [Obsolete("Use AddDependencyInjectionModules(...) or ScanAssemblies(...) instead. This should be removed in v3.0.", false)]
        public static IScanningContext FromAssemblyOf<T>(this IScanningContext scanningContext)
        {
            var assemblyToScan = typeof(T).Assembly;
            return scanningContext.ScanAssemblies(assemblyToScan);
        }

        /// <summary>
        /// Register the specified <see cref="IConfiguration"/>, used during DI module instantiation.
        /// </summary>
        /// <param name="scanningContext">The scanning context to add <see cref="IConfiguration"/> to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to add.</param>
        /// <returns>The <paramref name="scanningContext"/>.</returns>
        [Obsolete("Use UseConfiguration(...) instead. This should be removed in v3.0.", false)]
        public static IScanningContext WithConfiguration(this IScanningContext scanningContext, IConfiguration configuration)
        {
            return scanningContext.UseConfiguration(configuration);
        }
    }
}
