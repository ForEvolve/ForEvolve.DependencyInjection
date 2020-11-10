using ForEvolve.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TypeInfoCollectionExtensions
    {
        public static IEnumerable<TypeInfo> KeepOnlyDependencyInjectionModules(this IEnumerable<TypeInfo> types)
        {
            return types.Where(t => t.ImplementedInterfaces.Any(@interface => @interface == typeof(IDependencyInjectionModule)));
        }
    }
}
