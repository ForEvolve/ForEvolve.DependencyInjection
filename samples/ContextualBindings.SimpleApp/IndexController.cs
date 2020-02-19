using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextualBindings.SimpleApp
{
    [Route("")]
    public class IndexController : ControllerBase
    {
        public string BaseUrl { get; set; } = "https://localhost:5001";

        [HttpGet]
        public IEnumerable<string> Get()
        {
            yield return $"{BaseUrl}/api/first-controller-context";
            yield return $"{BaseUrl}/api/second-controller-context";
            yield return $"{BaseUrl}/api/firstname";
            yield return $"{BaseUrl}/api/lastname";
            yield return $"{BaseUrl}/api/fullname";
            yield return $"{BaseUrl}/api/complex-object-tree";
        }
    }
}
