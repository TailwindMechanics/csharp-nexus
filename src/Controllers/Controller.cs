//path: src\Controllers\Controller.cs

using Microsoft.AspNetCore.Mvc;

using Neurocache.Broadcasts;
using Neurocache.ShipsInfo;
using Neurocache.Schema;
using Neurocache.Hubs;

namespace Neurocache.Controllers
{
    [ApiController]
    public class Controller : ControllerBase
    {
        [HttpPost("kill")]
        public IActionResult Kill()
        {
            Ships.Log("Killing all operations");

            BroadcastChannelService.Kill();
            HubOperationService.Kill();
            return Ok();
        }

        [HttpPost("operation/stop")]
        public IActionResult Stop([FromBody] StopOperationRequest body)
        {
            body.Deconstruct(out var token);

            var operationToken = Guid.Parse(token);
            if (operationToken == Guid.Empty)
            {
                Ships.Log($"Invalid operation token: {token}");
                return BadRequest();
            }

            Ships.Log($"Stopping operation: {operationToken}");
            BroadcastChannelService.Stop(operationToken);
            HubOperationService.Stop(operationToken);
            return Ok();
        }

        [HttpGet("broadcast/{token}")]
        public async Task<IActionResult> Broadcast(string token)
        {
            Ships.Log($"Cruiser ready to receive broadcasts for token: {token}");

            var operationToken = Guid.Parse(token);
            if (operationToken == Guid.Empty)
            {
                Ships.Log($"Invalid operation token: {token}");
                return BadRequest();
            }

            Response.StatusCode = 200;
            Response.ContentType = "application/json";

            var broadcastChannel = BroadcastChannelService.OpenChannel(
                Request,
                Response,
                HttpContext,
                operationToken
            );

            await Task.WhenAll(broadcastChannel.ReadingTask, broadcastChannel.WritingTask);
            return new EmptyResult();
        }
    }
}