//path: src\Hubs\Hub.cs

using System.Reactive.Linq;
using System.Reactive;

using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public static class Hub
    {
        public static IObservable<HubOperation?> Run(string nodeId, HubOperation hubOperation, Func<Task> callback)
            => Observable
                .FromAsync(callback)
                .Select(_ => hubOperation)
                .TakeUntil(StopStream(nodeId, hubOperation))
                .LastOrDefaultAsync();

        static IObservable<Unit> StopStream(string nodeId, HubOperation hubOperation)
            => Observable.If(() => !hubOperation.OperationReport.Dependents.Contains(nodeId),
                Observable.Return(Unit.Default),
                hubOperation.StopStream
                    .Where(token => token == hubOperation.OperationReport.Token)
                    .Select(_ => Unit.Default)
                    .Merge(hubOperation.KillStream)
                    .Take(1));
    }
}