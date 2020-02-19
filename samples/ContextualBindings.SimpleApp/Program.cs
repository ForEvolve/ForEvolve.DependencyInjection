using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ContextualBindings.SimpleApp.ContextualControllerInjection;
using ContextualBindings.SimpleApp.ContextualServiceInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ContextualBindings.SimpleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureServices(services =>
                    {
                        services
                            // Not working yet
                            .AddContextualControllerInjection()

                            // Working
                            .AddContextualServiceInjection()
                            .AddComplexObjectTree()
                        ;


                        // MVC
                        services.AddControllers();
                        services.Decorate<IControllerActivator, ControllerActivator>();
                    })
                    .Configure(app => app
                        .UseDeveloperExceptionPage()
                        .UseRouting()
                        .UseEndpoints(endpoints => endpoints.MapControllers())
                    )
                )
                .Build()
                .Run();
        }
    }

    public class ControllerActivator : IControllerActivator
    {
        private readonly IControllerActivator _controllerActivator;

        public ControllerActivator(IControllerActivator controllerActivator)
        {
            _controllerActivator = controllerActivator ?? throw new ArgumentNullException(nameof(controllerActivator));
        }

        public object Create(ControllerContext controllerContext)
        {
            var result = _controllerActivator.Create(controllerContext);
            return result;
        }

        public void Release(ControllerContext context, object controller)
        {
            _controllerActivator.Release(context, controller);
        }
    }
}
