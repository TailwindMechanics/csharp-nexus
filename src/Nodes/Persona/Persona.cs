//path: src\Nodes\Persona\Persona.cs

using System.Reactive.Linq;

using Neurocache.Csharp.Nexus.Utilities;
using Neurocache.Csharp.Nexus.Schema;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public class Persona : INode
    {
        public static string NodeId => nameof(Persona).ToLower();
        public static async void Run(NodeRunArgs args)
            => await Node.Run(NodeId, args, async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                args.Callback.OnNext(new NodeRecord(
                    args.Bulletin.SessionToken,
                    "Something happened!",
                    NodeId,
                    true
                ));
            });
    }
}
