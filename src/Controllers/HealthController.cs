//path: src\Controllers\HealthController.cs

using Microsoft.AspNetCore.Mvc;

namespace Neurocache.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
            => Ok();
    }
}
