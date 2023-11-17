//path: src\Schema\INode.cs

namespace Neurocache.Csharp.Nexus.Schema
{
    public interface INode
    {
        static abstract string NodeId { get; }
        static abstract void Run(NodeRunArgs args);
    }
}