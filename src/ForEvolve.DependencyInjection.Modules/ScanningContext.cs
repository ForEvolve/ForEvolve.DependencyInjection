using ForEvolve.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ForEvolve.DependencyInjection
{
    public class ScanningContext : IScanningContext
    {
        protected IServiceCollection LocalServices { get; } = new ServiceCollection();
        private readonly Type _moduleType = typeof(IDependencyInjectionModule);

        public ScanningContext(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
            LocalServices.TryAddSingleton(services);
        }

        public IScanningContext WithDependencies(Action<IServiceCollection> setup)
        {
            if (setup == null) { throw new ArgumentNullException(nameof(setup)); }
            setup(LocalServices);
            return this;
        }

        public IScanningContext Initialize(IEnumerable<TypeInfo> allTypes)
        {
            using var serviceProvider = LocalServices.BuildServiceProvider();
            var modules = allTypes.Where(t => t.ImplementedInterfaces.Any(@interface => @interface == _moduleType));
            foreach (var module in modules)
            {
                ActivatorUtilities.CreateInstance(serviceProvider, module.AsType());
            }
            return this;
        }
    }
}
