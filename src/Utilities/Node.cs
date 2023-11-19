//path: src\Utilities\Node.cs

using System.Reactive.Linq;
using System.Reactive;

using Neurocache.Schema;

namespace Neurocache.Utilities
{
    public static class Node
    {
        public static IObservable<NodeRunArgs?> Run(string nodeId, NodeRunArgs args, Func<Task> callback)
            => Observable
                .FromAsync(callback)
                .Select(_ => args)
                .TakeUntil(StopStream(nodeId, args))
                .LastOrDefaultAsync();

        static IObservable<Unit> StopStream(string nodeId, NodeRunArgs args)
            => Observable.If(() => !args.Bulletin.NodeIds.Contains(nodeId),
                Observable.Return(Unit.Default),
                args.StopStream
                    .Where(token => token == args.Bulletin.SessionToken)
                    .Select(_ => Unit.Default)
                    .Merge(args.KillStream)
                    .Take(1));
    }
}