//path: src\Nodes\GptChat\GptChat.cs

using System.Reactive.Linq;

using Neurocache.Utilities;
using Neurocache.Schema;

namespace Neurocache.Nodes.OpenAi
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
                    NodeId,
                    false
                ));
                await Task.Delay(TimeSpan.FromSeconds(5));
                args.Callback.OnNext(new NodeRecord(
                    args.Bulletin.SessionToken,
                    "demo: async stream all message replies from gpt-4",
                    NodeId,
                    false
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
