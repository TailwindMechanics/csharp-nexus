//path: src\Schema\HubOperation.cs

using System.Reactive.Subjects;

namespace Neurocache.Schema
{
    public record OperationReport(string Token, string Author, string Payload, bool Final, List<string> Dependents);
    public record StopOperationRequest(string OperationToken);
    public record HubOperation(
        OperationReport OperationReport,
        ISubject<OperationReport> Callback,
        CancellationTokenSource CancelToken
    );
}