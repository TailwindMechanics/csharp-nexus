using Microsoft.AspNetCore.Mvc;
using Serilog;

using Neurocache.Utilities;

namespace Neurocache.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            var message = $"{VesselInfo.ThisVessel}: All systems normal.";
            Log.Information(message);
            return Ok(message);
        }
    }
}
