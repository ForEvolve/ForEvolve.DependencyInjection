using Microsoft.AspNetCore.Mvc;
using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    [Route("api/lastname")]
    public class LastNameController : ControllerBase
    {
        private readonly ILastNameService _lastNameService;
        public LastNameController(ILastNameService lastNameService)
        {
            _lastNameService = lastNameService ?? throw new ArgumentNullException(nameof(lastNameService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lastName = _lastNameService.GetLastName();
            return Ok(lastName);
        }
    }
}
