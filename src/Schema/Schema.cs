//path: src\Schema\Schema.cs

namespace Neurocache.Schema
{
    public record OperationReport(Guid Token, string Author, string Payload, bool Final, List<string> Dependents);
    public record StopOperationRequest(string OperationToken);
    public record HubOperation(OperationReport OperationReport, CancellationTokenSource CancelToken);
}