//path: src\Hubs\AvatarGen\AvatarGen.cs

using System.Reactive.Linq;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class AvatarGen : INode
    {
        public static string NodeId => nameof(AvatarGen).ToLower();
        public static async void Run(HubOperation hubOperation)
            => await Hub.Run(NodeId, hubOperation, async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Ships.Log("Generating avatar");
                hubOperation.Callback.OnNext(new OperationReport(
                    hubOperation.OperationReport.Token,
                    NodeId,
                    "A thing happened",
                    true,
                    []
                ));
            });
    }
}
