//path: src\Nodes\GptChat\GptChat.cs

using System.Reactive.Linq;

using Neurocache.Csharp.Nexus.Utilities;
using Neurocache.Csharp.Nexus.Schema;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public class GptChat : INode
    {
        public static string NodeId => nameof(GptChat).ToLower();
        public static async void Run(NodeRunArgs args)
            => await Node.Run(NodeId, args, async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                args.Callback.OnNext(new NodeRecord(
                    args.Bulletin.SessionToken,
                    "demo: async fetch memories from pinecone",
                    NodeId
                ));
                await Task.Delay(TimeSpan.FromSeconds(5));
                args.Callback.OnNext(new NodeRecord(
                    args.Bulletin.SessionToken,
                    "demo: async stream all message replies from gpt-4",
                    NodeId
                ));
                await Task.Delay(TimeSpan.FromSeconds(5));
                args.Callback.OnNext(new NodeRecord(
                    args.Bulletin.SessionToken,
                    "demo: async update pinecone with new memories",
                    NodeId,
                    true
                ));
            });
    }
}
