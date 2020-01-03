using ForEvolve.DependencyInjection.ContextualBindings;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContextualBindingsExtensions
    {
        public static ContextualServiceDescriptor AddContextualBinding<TService>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
        {
            return services.AddContextualBinding<TService, TService>(lifetime);
        }

        public static ContextualServiceDescriptor AddContextualBinding<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
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
            return contextualServiceDescriptor;
        }

        public static ContextualServiceDescriptor WithConstructorArgument<TService, TImplementation>(this ContextualServiceDescriptor contextualServiceDescriptor, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
            where TImplementation : class, TService
        {
            return WithConstructorArgument<TService, TImplementation>(
                contextualServiceDescriptor, 
                default, 
                lifetime
            );
            //var argumentDescriptor = contextualServiceDescriptor
            //    .ContextualServices
            //    .AddContextualBinding<TService, TImplementation>(lifetime);
            //contextualServiceDescriptor.ConstructorArguments.Add(argumentDescriptor);
            //contextualServiceDescriptor
            //    .ContextualServices.TryAdd(
            //        ServiceDescriptor.Describe(
            //            typeof(TImplementation),
            //            typeof(TImplementation),
            //            lifetime
            //        )
            //    );

            ////contextualServiceDescriptor.ConstructorArguments.Add(new ConstructorArgument(
            ////    typeof(TService),
            ////    typeof(TImplementation)
            ////));
            ////contextualServiceDescriptor.Services.TryAdd(
            ////    ServiceDescriptor.Describe(
            ////        typeof(TImplementation),
            ////        typeof(TImplementation),
            ////        lifetime
            ////    )
            ////);
            //return contextualServiceDescriptor;
        }

        public static ContextualServiceDescriptor WithConstructorArgument<TService, TImplementation>(this ContextualServiceDescriptor contextualServiceDescriptor, Action<ContextualServiceDescriptor> argumentSetup, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
            where TImplementation : class, TService
        {
            var argumentDescriptor = contextualServiceDescriptor
                .ContextualServices
                .AddContextualBinding<TService, TImplementation>(lifetime);
            contextualServiceDescriptor.ConstructorArguments.Add(argumentDescriptor);
            contextualServiceDescriptor
                .ContextualServices.TryAdd(
                    ServiceDescriptor.Describe(
                        typeof(TImplementation),
                        typeof(TImplementation),
                        lifetime
                    )
                );
            argumentSetup?.Invoke(argumentDescriptor);
            return contextualServiceDescriptor;
        }
    }
}
