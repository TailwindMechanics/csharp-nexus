//path: src\NodeRouter\INode.cs

namespace Neurocache.Csharp.Nexus.NodeRouter
{
    public interface INode
    {
        public static readonly string? NodeId;
        public Task<NodeRecord> RunAsync(string input);
    }
}