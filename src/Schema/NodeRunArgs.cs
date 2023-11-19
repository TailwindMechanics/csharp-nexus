//path: src\Schema\NodeRunArgs.cs

using System.Reactive.Subjects;
using System.Reactive;

namespace Neurocache.Schema
{
    public record NodeRunArgs(
        Bulletin Bulletin,
        ISubject<NodeRecord> Callback,
        IObservable<string> StopStream,
        IObservable<Unit> KillStream
    );
}