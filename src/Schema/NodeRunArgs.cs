//path: src\Schema\NodeRunArgs.cs

using System.Reactive.Subjects;
using System.Reactive;

namespace Neurocache.Schema
{
    public record HubOperation(
        OperationReport OperationReport,
        ISubject<OperationReport> Callback,
        IObservable<string> StopStream,
        IObservable<Unit> KillStream
    );
}