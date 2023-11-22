//path: src\Controllers\Controller.cs

using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using System.Reactive.Linq;
using System.Reactive;
using Newtonsoft.Json;
using System.Text;
using Serilog;

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
            BulletinRouter.KillSubject.OnNext(Unit.Default);
            return Ok();
        }

        [HttpPost("stop")]
        public IActionResult Stop([FromBody] StopSessionRequest body)
        {
            Ships.Log("Stopping session");
            body.Deconstruct(out var sessionToken);
            BulletinRouter.StopSubject.OnNext(sessionToken);
            return Ok();
        }

        [HttpPost("run")]
        public async Task Run()
        {
            Ships.Log($"Received operation request");

            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var bulletin = JsonConvert.DeserializeObject<Bulletin>(body)!;

            int completedNodeCount = 0;
            var channel = Channel.CreateUnbounded<string>();

            BulletinRouter.RecordStream
                .Where(rec => rec.SessionToken == bulletin.SessionToken)
                .Subscribe(rec =>
                {
                    if (rec.IsFinal) completedNodeCount++;

                    var serialized = JsonConvert.SerializeObject(rec);
                    channel.Writer.TryWrite(serialized);

                    Ships.Log($"Emitting report:{rec.Payload}, from:{rec.NodeId}, is final:{rec.IsFinal}");
                    if (completedNodeCount >= bulletin.NodeIds.Count)
                    {
                        Ships.Log("All nodes completed, closing channel");
                        channel.Writer.TryComplete();
                    }
                });

            BulletinRouter.BulletinSubject.OnNext(bulletin);

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