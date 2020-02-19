using Microsoft.Extensions.DependencyInjection;
using System;

namespace ForEvolve.DependencyInjection.ContextualBindings
{
    public class ConstructorArgument
    {
        public ConstructorArgument(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public Type ServiceType { get; }
        public Type ImplementationType { get; }
    }
}
