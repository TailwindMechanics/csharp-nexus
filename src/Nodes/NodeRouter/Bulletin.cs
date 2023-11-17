//path: src\Nodes\NodeRouter\Bulletin.cs

namespace Neurocache.Csharp.Nexus.Nodes.NodeRouter
{
    public record Bulletin(string Payload, List<string> NodeIds);
}