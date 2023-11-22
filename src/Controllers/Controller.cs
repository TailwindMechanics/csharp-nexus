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
        [HttpPost("kill")]
        public IActionResult Kill()
        {
            Ships.Log("Killing all sessions");
            DispatchForwarder.KillSubject.OnNext(Unit.Default);
            return Ok();
        }

        [HttpPost("operation/stop")]
        public IActionResult Stop([FromBody] StopOperationRequest body)
        {
            Ships.Log("Stopping session");
            body.Deconstruct(out var sessionToken);
            DispatchForwarder.StopSubject.OnNext(sessionToken);
            return Ok();
        }

        [HttpPost("operation/dispatch")]
        public async Task Run()
        {
            Ships.Log($"Received dispatch");

            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var dispatchReport = JsonConvert.DeserializeObject<OperationReport>(body)!;

            int completedNodeCount = 0;
            var channel = Channel.CreateUnbounded<string>();

            DispatchForwarder.ReportStream
                .Where(report => report.Token == dispatchReport.Token)
                .Subscribe(report =>
                {
                    if (report.Final) completedNodeCount++;

                    var serialized = JsonConvert.SerializeObject(report);
                    channel.Writer.TryWrite(serialized);

                    Ships.Log($"Emitting report:{report.Payload}, from:{report.Author}, is final:{report.Final}");
                    if (completedNodeCount >= dispatchReport.Dependents.Count)
                    {
                        Ships.Log("All nodes completed, closing channel");
                        channel.Writer.TryComplete();
                    }
                });

            DispatchForwarder.DispatchSubject.OnNext(dispatchReport);

            var stream = Response.Body;
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            await foreach (var rec in channel.Reader.ReadAllAsync())
            {
                await stream.WriteAsync(Encoding.UTF8.GetBytes(rec));
                await stream.FlushAsync();
            }
        }
    }
}