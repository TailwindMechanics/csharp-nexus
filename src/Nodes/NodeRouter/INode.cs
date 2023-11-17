//path: src\Nodes\NodeRouter\INode.cs

namespace Neurocache.Csharp.Nexus.Nodes.NodeRouter
{
    public interface INode
    {
        public static readonly string? NodeId;
        public Task<NodeRecord> RunAsync(string input);
    }
}