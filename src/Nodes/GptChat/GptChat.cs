//path: src\Nodes\GptChat\GptChat.cs

using Serilog;

using Neurocache.Csharp.Nexus.Nodes.NodeRouter;

namespace Neurocache.Csharp.Nexus.Nodes.OpenAi
{
    public static class GptChat
    {
        public static async Task<NodeRecord> RunAsync(string agentId, string input)
        {
            Log.Information(input);
            await Task.Delay(1000);
            return new NodeRecord(agentId, input);
        }
    }
}
