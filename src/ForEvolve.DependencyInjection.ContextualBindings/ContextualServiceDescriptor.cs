using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));

            object factory(IServiceProvider serviceProvider)
            {
                var nestedProvider = BuildNestedProvider(serviceProvider);
                var serviceTypes = ConstructorArguments
                    .Select(x => x.ServiceType)
                    .ToArray();
                var arguments = ConstructorArguments
                    .Select(x =>
                    {
                        var provider = x.BuildNestedProvider(nestedProvider);
                        var service = provider.GetService(x.ImplementationType);
                        return service;
                    })
                    .ToArray();
                var f = ActivatorUtilities.CreateFactory(implementationType, serviceTypes);
                return f(nestedProvider, arguments);
            }
        }

        public Type ServiceType { get; }
        public Type ImplementationType { get; }

        public IServiceCollection Services { get; }
        public IServiceCollection ContextualServices { get; } = new ServiceCollection();
        public ServiceDescriptor ServiceDescriptor { get; }
        public List<ContextualServiceDescriptor> ConstructorArguments { get; } = new List<ContextualServiceDescriptor>();

        private IServiceProvider _nestedServiceProvider = default;

        private IServiceProvider BuildNestedProvider(params IServiceProvider[] serviceProviders)
        {
            if(_nestedServiceProvider == default)
            {
                var contextualProvider = ContextualServices.BuildServiceProvider();
                var list = new List<IServiceProvider>(serviceProviders);
                list.Insert(0, contextualProvider);
                _nestedServiceProvider = new MultipleServiceProvider(list);
            }
            return _nestedServiceProvider;
        }
    }

    public class MultipleServiceProvider : IServiceProvider
    {
        private readonly List<IServiceProvider> _serviceProviders;
        public MultipleServiceProvider(IEnumerable<IServiceProvider> serviceProviders)
        {
            if (serviceProviders == null) { throw new ArgumentNullException(nameof(serviceProviders)); }
            _serviceProviders = new List<IServiceProvider>(serviceProviders);
        }

        public void Add(IServiceProvider serviceProvider)
            => _serviceProviders.Add(serviceProvider);

        public void AddRange(IEnumerable<IServiceProvider> serviceProviders) 
            => _serviceProviders.AddRange(serviceProviders);

        public object GetService(Type serviceType)
        {
            foreach (var serviceProvider in _serviceProviders)
            {
                try
                {
                    var service = serviceProvider.GetService(serviceType);
                    if (service != default)
                    {
                        return service;
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
            return null;
        }
    }
}
