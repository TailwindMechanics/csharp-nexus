//path: src\Schema\Bulletin.cs

namespace Neurocache.Schema
{
    public record Bulletin(string SessionToken, string Payload, List<string> NodeIds);
}