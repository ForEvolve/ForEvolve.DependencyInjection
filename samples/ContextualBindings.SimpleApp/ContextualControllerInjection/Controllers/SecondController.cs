using Microsoft.AspNetCore.Mvc;
using System;

namespace ContextualBindings.SimpleApp.ContextualControllerInjection
{
    [Route("api/second-controller-context")]
    public class SecondController : ControllerBase
    {
        private readonly IService _service;

        public SecondController(IService service)
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
