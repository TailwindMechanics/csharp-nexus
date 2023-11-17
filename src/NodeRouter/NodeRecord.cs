//path: src\NodeRouter\NodeRecord.cs

namespace Neurocache.Csharp.Nexus.NodeRouter
{
    public record NodeRecord(string SessionToken, string Payload, string NodeId, bool IsFinal = false);
}