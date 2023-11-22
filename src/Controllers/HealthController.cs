//path: src\Controllers\HealthController.cs

using Microsoft.AspNetCore.Mvc;

using Neurocache.ShipsInfo;

namespace Neurocache.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            Ships.Log("All systems normal");
            return Ok();
        }
    }
}
