using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForEvolve.DependencyInjection.ContextualBindings
{
    public class ContextualServiceDescriptor
    {
        public ContextualServiceDescriptor(IServiceCollection services,
            Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }
            if (implementationType == null) { throw new ArgumentNullException(nameof(implementationType)); }
            Services = services ?? throw new ArgumentNullException(nameof(services));

            ServiceDescriptor = new ServiceDescriptor(serviceType, factory, lifetime);

            object factory(IServiceProvider serviceProvider)
            {
                var serviceTypes = ConstructorArguments
                    .Select(x => x.ServiceType)
                    .ToArray();
                var arguments = ConstructorArguments
                    .Select(x => serviceProvider.GetService(x.ImplementationType))
                    .ToArray();
                var f = ActivatorUtilities.CreateFactory(implementationType, serviceTypes);
                return f(serviceProvider, arguments);
            }
        }

        public IServiceCollection Services { get; }
        public ServiceDescriptor ServiceDescriptor { get; }
        public List<ConstructorArgument> ConstructorArguments { get; } = new List<ConstructorArgument>();
    }
}
