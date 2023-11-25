//path: src\Controllers\Controller.cs

using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using System.Reactive.Linq;
using System.Reactive;
using Newtonsoft.Json;
using System.Text;

using Neurocache.NodeRouter;
using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Controllers
{
    [ApiController]
    public class Controller : ControllerBase
    {
        CancellationTokenSource CancelToken = new();

        [HttpPost("kill")]
        public IActionResult Kill()
        {
            Ships.Log("Killing all operations");

            DispatchForwarder.Kill();

            return Ok();
        }

        [HttpPost("operation/stop")]
        public IActionResult Stop([FromBody] StopOperationRequest body)
        {
            body.Deconstruct(out var operationToken);

            Ships.Log($"Stopping operation: {operationToken}");

            DispatchForwarder.Stop(operationToken);
            return Ok();
        }

        [HttpGet("broadcast/{token}")]
        public async Task<IActionResult> BroadcastChannel(string token)
        {
            Ships.Log($"Cruiser ready to receive broadcasts for token: {token}");

            var channel = Channel.CreateUnbounded<string>();
            DispatchForwarder.ReportStream
                .Where(report => report.Token == token)
                .Subscribe(report =>
                {
                    var serialized = JsonConvert.SerializeObject(report);
                    channel.Writer.TryWrite(serialized);

                    Ships.Log($"Cruiser emitting report: {report.Payload}, from: {report.Author}, is final: {report.Final}");
                    if (report.Final)
                    {
                        channel.Writer.TryComplete();
                        Ships.Log("Final report received, closing broadcast channel");
                    }
                },
                onError: error =>
                {
                    channel.Writer.TryComplete();
                },
                onCompleted: () =>
                {
                    channel.Writer.TryComplete();
                });

            var cancelToken = HttpContext.RequestAborted;
            DispatchForwarder.InitializeDispatch(token, cancelToken);

            var stream = Response.Body;
            Response.StatusCode = 200;
            Response.ContentType = "application/json";

            await foreach (var rec in channel.Reader.ReadAllAsync(cancelToken))
            {
                await stream.WriteAsync(Encoding.UTF8.GetBytes(rec), cancelToken);
                await stream.FlushAsync(cancelToken);
            }

            return new EmptyResult();
        }
    }
}