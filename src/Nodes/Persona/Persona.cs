//path: src\Nodes\Persona\Persona.cs

using System.Reactive.Subjects;
using Serilog;

using Neurocache.Csharp.Nexus.NodeRouter;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public static class Persona
    {
        public static string NodeId => nameof(Persona).ToLower();
        public static async void RunAsync(string sessionToken, string payload, ISubject<NodeRecord> callback)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
            callback.OnNext(new NodeRecord(sessionToken, payload, NodeId, true));
        }
    }
}
