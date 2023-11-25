//path: src\Hubs\AvatarGen\AvatarGen.cs

using System.Reactive.Linq;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class AvatarGen : IHub
    {
        public static string HubId => nameof(AvatarGen).ToLower();
        public static async void Run(HubOperation hubOperation)
            => await Hub.Run(HubId, hubOperation, async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Ships.Log("Generating avatar");
                hubOperation.Callback.OnNext(new OperationReport(
                    hubOperation.OperationReport.Token,
                    HubId,
                    "A thing happened",
                    true,
                    []
                ));
            });
    }
}
