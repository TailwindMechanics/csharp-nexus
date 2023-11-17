//path: src\Controllers\BulletinController.cs

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

using Neurocache.Csharp.Nexus.Nodes.NodeRouter;

namespace Neurocache.Csharp.Nexus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BulletinController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Bulletin()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var bulletin = JsonConvert.DeserializeObject<Bulletin>(body)!;

            Log.Information($"Received bulletin with payload: {bulletin.Payload}");
            NodeRouter.OnBulletinSubject.OnNext(bulletin);

            return Ok();
        }
    }
}