//path: src\Nodes\AvatarGen\AvatarGen.cs

using System.Reactive.Linq;

using Neurocache.Csharp.Nexus.Utilities;
using Neurocache.Csharp.Nexus.Schema;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
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
