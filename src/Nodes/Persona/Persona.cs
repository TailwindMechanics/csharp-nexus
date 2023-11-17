//path: src\Nodes\Persona\Persona.cs

using System.Reactive.Subjects;

using Neurocache.Csharp.Nexus.Schema;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public class Persona : INode
    {
        public static string NodeId => nameof(Persona).ToLower();
        public static async void Run(string sessionToken, string payload, ISubject<NodeRecord> callback)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
            callback.OnNext(new NodeRecord(sessionToken, payload, NodeId, true));
        }
    }
}
