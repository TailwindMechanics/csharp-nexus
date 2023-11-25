//path: src\Hubs\GptChat\GptChat.cs

using Neurocache.Broadcasts;
using Neurocache.ShipsInfo;
using Neurocache.Utilities;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public static class GptChat
    {
        public static string HubId => nameof(GptChat).ToLower();
        public static async void Run(HubOperation hubOperation)
        {
            Ships.Log("demo: async fetch memories from pinecone");

            await Utils.Wait(10, hubOperation);

            var hubReport = new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "demo: async fetch memories from pinecone",
                false,
                []
            );

            BroadcastChannelService.SendHubReportToVanguardStream.OnNext(hubReport);

            await Utils.Wait(5, hubOperation);

            Ships.Log("demo: async stream all message replies from gpt-4");

            hubReport = new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "demo: async stream all message replies from gpt-4",
                false,
                []
            );

            BroadcastChannelService.SendHubReportToVanguardStream.OnNext(hubReport);

            await Utils.Wait(5, hubOperation);

            Ships.Log("demo: async update pinecone with new memories");

            hubReport = new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "demo: async update pinecone with new memories",
                true,
                []
            );

            BroadcastChannelService.SendHubReportToVanguardStream.OnNext(hubReport);
        }
    }
}
