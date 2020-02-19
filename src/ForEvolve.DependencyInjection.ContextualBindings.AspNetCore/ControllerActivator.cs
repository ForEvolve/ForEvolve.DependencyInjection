using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;

namespace ForEvolve.DependencyInjection.ContextualBindings.AspNetCore
{
    public class ControllerActivator : IControllerActivator
    {
        private readonly IControllerActivator _controllerActivator;

        public ControllerActivator(IControllerActivator controllerActivator)
        {
            _controllerActivator = controllerActivator ?? throw new ArgumentNullException(nameof(controllerActivator));
        }

        public object Create(ControllerContext controllerContext)
        {
            // Try to activate the controller using service provider
            var type = controllerContext.ActionDescriptor.ControllerTypeInfo.AsType();
            var result = controllerContext.HttpContext.RequestServices.GetService(type);

            // Fallback to the default IControllerActivator 
            if (result == null)
            {
                result = _controllerActivator.Create(controllerContext);
            }

            return result;
        }

        public void Release(ControllerContext context, object controller)
        {
            _controllerActivator.Release(context, controller);
        }
    }
}
