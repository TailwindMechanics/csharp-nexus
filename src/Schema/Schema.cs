//path: src\Schema\HubOperation.cs

using System.Reactive.Subjects;
using System.Reactive;

namespace Neurocache.Schema
{
    public record OperationReport(string Token, string Author, string Payload, bool Final, List<string> Dependents);
    public record StopOperationRequest(string SessionToken);
    public record HubOperation(
        OperationReport OperationReport,
        ISubject<OperationReport> Callback,
        IObservable<string> StopStream,
        IObservable<Unit> KillStream
    );
}