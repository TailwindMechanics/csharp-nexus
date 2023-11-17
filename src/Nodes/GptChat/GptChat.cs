//path: src\Nodes\GptChat\GptChat.cs

using System.Reactive.Subjects;
using Serilog;

using Neurocache.Csharp.Nexus.NodeRouter;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public static class GptChat
    {
        public static string NodeId => nameof(GptChat).ToLower();
        public static async void RunAsync(string sessionToken, string payload, ISubject<NodeRecord> callback)
        {
            await Task.Delay(TimeSpan.FromSeconds(4));
            callback.OnNext(new NodeRecord(sessionToken, payload, NodeId, true));
        }
    }
}
