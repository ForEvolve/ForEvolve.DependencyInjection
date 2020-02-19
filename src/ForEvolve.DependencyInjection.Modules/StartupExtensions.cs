using ForEvolve.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ForEvolveDependencyInjectionStartupExtensions
    {
        /// <summary>
        /// Create a <see cref="IScanningContext"/> ready to scan for DI modules.
        /// </summary>
        /// <param name="services">The service collection to add dependency to.</param>
        /// <returns>The newly created <see cref="IScanningContext"/>.</returns>
        public static IScanningContext ScanForDIModules(this IServiceCollection services)
        {
            return new ScanningContext(services);
        }

        /// <summary>
        /// Register the specified <see cref="IConfiguration"/>, used during DI module instanciation.
        /// </summary>
        /// <param name="scanningContext">The scanning context to add <see cref="IConfiguration"/> to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to add.</param>
        /// <returns>The <paramref name="scanningContext"/>.</returns>
        public static IScanningContext WithConfiguration(this IScanningContext scanningContext, IConfiguration configuration)
        {
            return scanningContext
                .WithDependencies(services => services.TryAddSingleton(configuration))
            ;
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
        public static IScanningContext FromAssemblyOf<T>(this IScanningContext scanningContext)
        {
            var assemblyToScan = typeof(T).Assembly;
            if (assemblyToScan.IsDynamic)
            {
                return scanningContext; // throw an exception?
            }
            var allTypes = assemblyToScan
                .DefinedTypes
                .Where(t => t.IsClass && !t.IsAbstract);
            scanningContext.Initialize(allTypes);
            return scanningContext;
        }
    }
}
