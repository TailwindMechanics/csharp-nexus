//path: src\Hubs\Persona\Persona.cs

using System.Reactive.Linq;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class Persona : INode
    {
        public static string NodeId => nameof(Persona).ToLower();
        public static async void Run(HubOperation hubOperation)
            => await Hub.Run(NodeId, hubOperation, async () =>
            {
                Ships.Log("Generating persona");
                await Task.Delay(TimeSpan.FromSeconds(10));
                hubOperation.Callback.OnNext(new OperationReport(
                    hubOperation.OperationReport.Token,
                    NodeId,
                    "Something happened!",
                    true,
                    []
                ));
            });
    }
}
