//path: src\Nodes\AvatarGen\AvatarGen.cs

using System.Reactive.Linq;

using Neurocache.Utilities;
using Neurocache.Schema;

namespace Neurocache.Nodes.OpenAi
{
    public class AvatarGen : INode
    {
        public static string NodeId => nameof(AvatarGen).ToLower();
        public static async void Run(NodeRunArgs args)
            => await Node.Run(NodeId, args, async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                args.Callback.OnNext(new NodeRecord(
                    args.Bulletin.SessionToken,
                    "A thing happened",
                    NodeId,
                    true
                ));
            });
    }
}
