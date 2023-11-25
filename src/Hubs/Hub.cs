//path: src\Hubs\Hub.cs

using System.Reactive.Linq;
using System.Reactive;

using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public static class Hub
    {
        public static IObservable<HubOperation?> Run(string nodeId, HubOperation hubOperation, Func<Task> callback)
        {
            return Observable
            .FromAsync(callback)
            .Select(_ => hubOperation)
            .Merge(hubOperation.StopStream
                        .Where(token => token == hubOperation.OperationReport.Token)
                        .Select(_ => (HubOperation?)null))
            .TakeUntil(hubOperation.KillStream)
            .Where(operation => operation != null && operation.OperationReport.Dependents.Contains(nodeId));
        }
    }
}