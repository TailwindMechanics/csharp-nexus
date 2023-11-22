//path: src\Schema\HubOperation.cs

namespace Neurocache.Schema
{
    public record OperationReport(string Token, string Author, string Payload, bool Final, List<string> Dependents);
    public record StopOperationRequest(string SessionToken);
}