//path: src\NodeRouter\Bulletin.cs

namespace Neurocache.Csharp.Nexus.NodeRouter
{
    public record Bulletin(string SessionToken, string Payload, List<string> NodeIds);
}