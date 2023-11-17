//path: src\Nodes\AvatarGen\AvatarGen.cs

using System.Reactive.Subjects;
using Serilog;

using Neurocache.Csharp.Nexus.NodeRouter;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public static class AvatarGen
    {
        public static string NodeId => nameof(AvatarGen).ToLower();
        public static async void RunAsync(string sessionToken, string payload, ISubject<NodeRecord> callback)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            callback.OnNext(new NodeRecord(sessionToken, payload, NodeId));

            await Task.Delay(TimeSpan.FromSeconds(15));
            callback.OnNext(new NodeRecord(sessionToken, payload, NodeId));

            await Task.Delay(TimeSpan.FromSeconds(20));
            callback.OnNext(new NodeRecord(sessionToken, payload, NodeId, true));
        }
    }
}
