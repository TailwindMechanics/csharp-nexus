//path: src\Nodes\GptChat\GptChat.cs

using System.Reactive.Subjects;

using Neurocache.Csharp.Nexus.Schema;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public class GptChat : INode
    {
        public static string NodeId => nameof(GptChat).ToLower();
        public static async void Run(string sessionToken, string payload, ISubject<NodeRecord> callback)
        {
            await Task.Delay(TimeSpan.FromSeconds(4));
            callback.OnNext(new NodeRecord(sessionToken, payload, NodeId, true));
        }
    }
}
