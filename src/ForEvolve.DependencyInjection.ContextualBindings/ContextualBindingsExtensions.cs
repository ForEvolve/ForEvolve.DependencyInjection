using ForEvolve.DependencyInjection.ContextualBindings;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContextualBindingsExtensions
    {
        public static IServiceCollection AddContextualBinding<TService>(this IServiceCollection services, Action<ContextualServiceDescriptor> setup = null, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
        {
            return services.AddContextualBinding<TService, TService>(setup, lifetime);
        }

        public static IServiceCollection AddContextualBinding<TService, TImplementation>(this IServiceCollection services, Action<ContextualServiceDescriptor> setup = null, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
            where TImplementation : class, TService
        {
            var contextualServiceDescriptor = new ContextualServiceDescriptor(
                services,
                typeof(TService),
                typeof(TImplementation),
                lifetime
            );
            services.Add(contextualServiceDescriptor.ServiceDescriptor);
            setup?.Invoke(contextualServiceDescriptor);
            return services;
        }

        public static ContextualServiceDescriptor WithConstructorArgument<TService, TImplementation>(this ContextualServiceDescriptor contextualServiceDescriptor, Action<ContextualServiceDescriptor> setup = null, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
            where TImplementation : class, TService
        {
            contextualServiceDescriptor.ConstructorArguments.Add(new ConstructorArgument(
                typeof(TService),
                typeof(TImplementation)
            ));
            contextualServiceDescriptor.Services
                .AddContextualBinding<TImplementation>(csd =>
                {
                    setup?.Invoke(csd);
                }, lifetime);
            return contextualServiceDescriptor;
        }
    }
}
