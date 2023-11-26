//path: src\Schema\Schema.cs

namespace Neurocache.Schema
{
    public record Ship(string Name, int Port);
    public record StopOperationRequest(string OperationToken);
    public record HubOperation(OperationReport OperationReport, CancellationTokenSource CancelToken);
}