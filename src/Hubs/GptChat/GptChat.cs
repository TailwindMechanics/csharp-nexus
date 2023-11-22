//path: src\Hubs\GptChat\GptChat.cs

using System.Reactive.Linq;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class GptChat : INode
    {
        public static string NodeId => nameof(GptChat).ToLower();
        public static async void Run(HubOperation hubOperation)
            => await Hub.Run(NodeId, hubOperation, async () =>
            {
                Ships.Log("demo: async fetch memories from pinecone");
                await Task.Delay(TimeSpan.FromSeconds(10));
                hubOperation.Callback.OnNext(new OperationReport(
                    hubOperation.OperationReport.Token,
                    NodeId,
                    "demo: async fetch memories from pinecone",
                    false,
                    []
                ));
                await Task.Delay(TimeSpan.FromSeconds(5));
                Ships.Log("demo: async stream all message replies from gpt-4");
                hubOperation.Callback.OnNext(new OperationReport(
                    hubOperation.OperationReport.Token,
                    NodeId,
                    "demo: async stream all message replies from gpt-4",
                    false,
                    []
                ));
                await Task.Delay(TimeSpan.FromSeconds(5));
                Ships.Log("demo: async update pinecone with new memories");
                hubOperation.Callback.OnNext(new OperationReport(
                    hubOperation.OperationReport.Token,
                    NodeId,
                    "demo: async update pinecone with new memories",
                    true,
                    []
                ));
            });
    }
}
