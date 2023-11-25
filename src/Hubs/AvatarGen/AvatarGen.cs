//path: src\Hubs\AvatarGen\AvatarGen.cs

using Neurocache.Broadcasts;
using Neurocache.ShipsInfo;
using Neurocache.Utilities;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class AvatarGen
    {
        public static string HubId => nameof(AvatarGen).ToLower();
        public static async void Run(HubOperation hubOperation)
        {
            Ships.Log("Generating avatar...");

            await Utils.Wait(1, hubOperation);

            Ships.Log("Finished generating avatar");
            var hubReport = new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "Generating avatar",
                false,
                []
            );

            BroadcastChannelService.SendHubReportToVanguardStream.OnNext(hubReport);
        }
    }
}
