//path: src\NodeRouter\BulletinRouter.cs

using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive;

using Neurocache.Csharp.Nexus.Nodes.OpenAi;
using Neurocache.Csharp.Nexus.Schema;

namespace Neurocache.Csharp.Nexus.NodeRouter
{
    public class BulletinRouter
    {
        private static readonly BulletinRouter instance = new();
        public static BulletinRouter Instance => instance;
        private BulletinRouter() { }

        public static readonly ISubject<Unit> KillSubject
            = new Subject<Unit>();
        public static readonly ISubject<string> StopSubject
            = new Subject<string>();
        public static readonly ISubject<Bulletin> BulletinSubject
            = new Subject<Bulletin>();

        public static IObservable<NodeRecord> RecordStream => recordSubject;
        static readonly ISubject<NodeRecord> recordSubject = new Subject<NodeRecord>();

        public static void Init()
        {
            BulletinSubject.Subscribe(OnBulletin);
            StopSubject.Subscribe();
        }

        private static void OnBulletin(Bulletin bulletin)
        {
            var args = new NodeRunArgs(
                bulletin,
                recordSubject,
                StopSubject,
                KillSubject
            );

            AvatarGen.Run(args);
            GptChat.Run(args);
            Persona.Run(args);
        }
    }
}
