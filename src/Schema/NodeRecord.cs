//path: src\Schema\NodeRecord.cs

namespace Neurocache.Csharp.Nexus.Schema
{
    public record NodeRecord(string SessionToken, string Payload, string NodeId, bool IsFinal);
}