//path: src\Schema\NodeRecord.cs

namespace Neurocache.Schema
{
    public record NodeRecord(string SessionToken, string Payload, string NodeId, bool IsFinal);
}