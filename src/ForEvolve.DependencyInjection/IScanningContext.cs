using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ForEvolve.DependencyInjection
{
    public interface IScanningContext
    {
        IScanningContext WithDependencies(Action<IServiceCollection> setup);
        IScanningContext Initialize(IEnumerable<TypeInfo> allTypes);
    }
}
