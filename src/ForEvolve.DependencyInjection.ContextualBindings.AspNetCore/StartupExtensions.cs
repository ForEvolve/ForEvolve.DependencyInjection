using ForEvolve.DependencyInjection.ContextualBindings.AspNetCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IScanningContext ScanForContextualBindingsModule(this IServiceCollection services)
        {
            return new ScanningContext(services);
        }

        public static IScanningContext FromAssemblyOf<T>(this IScanningContext scanningContext)
        {
            throw new NotImplementedException();
            //scanningContext.Services.Scan(s => s.FromAssemblyOf<T>()
            //    .AddClasses(classes => classes.AssignableTo<ContextualBindingsModule>())
            //        .AsSelf()
            //        .WithSingletonLifetime()
            //);
        }

        public static IMvcBuilder WithContextualBindings(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services
                .Decorate<IControllerActivator, ControllerActivator>();
            return mvcBuilder;
        }
    }

    public interface IScanningContext
    {
        IServiceCollection Services { get; }
    }

    public class ScanningContext : IScanningContext
    {
        public IServiceCollection Services { get; }

        public ScanningContext(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}
