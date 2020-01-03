using Microsoft.AspNetCore.Mvc;
using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    [Route("api/firstname")]
    public class FirstNameController : ControllerBase
    {
        private readonly IFirstNameService _firstNameService;
        public FirstNameController(IFirstNameService firstNameService)
        {
            _firstNameService = firstNameService ?? throw new ArgumentNullException(nameof(firstNameService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var firstName = _firstNameService.GetFirstName();
            return Ok(firstName);
        }
    }
}
