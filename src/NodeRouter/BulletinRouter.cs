//path: src\NodeRouter\BulletinRouter.cs

using System.Reactive.Subjects;
using System.Reactive.Linq;

using Neurocache.Csharp.Nexus.Nodes.OpenAi;

namespace Neurocache.Csharp.Nexus.NodeRouter
{
    public class BulletinRouter
    {
        private static readonly BulletinRouter instance = new();
        public static BulletinRouter Instance => instance;
        private BulletinRouter() { }

        public static readonly ISubject<Bulletin> BulletinSubject
            = new Subject<Bulletin>();

        public static IObservable<NodeRecord> RecordStream => recordSubject;
        static readonly ISubject<NodeRecord> recordSubject = new Subject<NodeRecord>();

        public static void Init()
            => BulletinSubject.Subscribe(OnBulletin);

        private static void OnBulletin(Bulletin bulletin)
        {
            if (bulletin.NodeIds.Contains(AvatarGen.NodeId))
                AvatarGen.RunAsync(bulletin.SessionToken, bulletin.Payload, recordSubject);

            if (bulletin.NodeIds.Contains(GptChat.NodeId))
                GptChat.RunAsync(bulletin.SessionToken, bulletin.Payload, recordSubject);

            if (bulletin.NodeIds.Contains(Persona.NodeId))
                Persona.RunAsync(bulletin.SessionToken, bulletin.Payload, recordSubject);
        }
    }
}
