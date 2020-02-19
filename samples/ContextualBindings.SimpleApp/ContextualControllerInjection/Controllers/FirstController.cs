using Microsoft.AspNetCore.Mvc;
using System;

namespace ContextualBindings.SimpleApp.ContextualControllerInjection
{
    [Route("api/first-controller-context")]
    public class FirstController : ControllerBase
    {
        private readonly IService _service;

        public FirstController(IService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.Execute());
        }
    }
}
