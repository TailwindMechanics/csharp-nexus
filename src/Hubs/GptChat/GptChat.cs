//path: src\Hubs\GptChat\GptChat.cs

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class GptChat : IHub
    {
        public static string HubId => nameof(GptChat).ToLower();
        public static async void Run(HubOperation hubOperation)
        {
            Ships.Log("demo: async fetch memories from pinecone");

            await Wait(10, hubOperation);

            hubOperation.Callback.OnNext(new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "demo: async fetch memories from pinecone",
                false,
                []
            ));

            await Wait(5, hubOperation);

            Ships.Log("demo: async stream all message replies from gpt-4");

            hubOperation.Callback.OnNext(new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "demo: async stream all message replies from gpt-4",
                false,
                []
            ));

            await Wait(5, hubOperation);

            Ships.Log("demo: async update pinecone with new memories");

            hubOperation.Callback.OnNext(new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "demo: async update pinecone with new memories",
                true,
                []
            ));
        }

        static Task Wait(float time, HubOperation hubOperation)
            => Task.Delay(TimeSpan.FromSeconds(time),
                hubOperation.CancelToken.Token);
    }
}
