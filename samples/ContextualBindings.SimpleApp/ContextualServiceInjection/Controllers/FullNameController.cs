using Microsoft.AspNetCore.Mvc;
using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    [Route("api/fullname")]
    public class FullNameController : ControllerBase
    {
        private readonly INameGenerator _nameGenerator;
        public FullNameController(INameGenerator nameGenerator)
        {
            _nameGenerator = nameGenerator ?? throw new ArgumentNullException(nameof(nameGenerator));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var name = _nameGenerator.Generate();
            return Ok(name);
        }
    }
}
