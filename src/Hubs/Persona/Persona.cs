//path: src\Hubs\Persona\Persona.cs

using Neurocache.Broadcasts;
using Neurocache.ShipsInfo;
using Neurocache.Utilities;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class Persona
    {
        public static string HubId => nameof(Persona).ToLower();
        public static async void Run(HubOperation hubOperation)
        {
            Ships.Log("Generating persona...");

            await Utils.Wait(10, hubOperation);

            Ships.Log("Finished generating persona");

            var hubReport = new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "Generating persona",
                false,
                []
            );

            BroadcastChannelService.SendHubReportToVanguardStream.OnNext(hubReport);
        }
    }
}
