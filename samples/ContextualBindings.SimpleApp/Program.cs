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
                            .AddDependencyInjectionModules()
                            .ScanAssemblies(typeof(Program).Assembly)
                            .Initialize()
                        ;

                        // MVC
                        services
                            .AddControllers()
                            .WithContextualBindings();
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
}
