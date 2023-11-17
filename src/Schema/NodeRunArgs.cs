//path: src\Schema\NodeRunArgs.cs

using System.Reactive.Subjects;
using System.Reactive;

namespace Neurocache.Csharp.Nexus.Schema
{
    public record NodeRunArgs(
        Bulletin Bulletin,
        ISubject<NodeRecord> Callback,
        IObservable<string> StopStream,
        IObservable<Unit> KillStream
    );
}