//path: src\Schema\Bulletin.cs

namespace Neurocache.Csharp.Nexus.Schema
{
    public record Bulletin(string SessionToken, string Payload, List<string> NodeIds);
}