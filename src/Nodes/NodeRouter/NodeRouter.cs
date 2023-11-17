//path: src\Nodes\NodeRouter\NodeRouter.cs

using System.Reactive.Subjects;
using System.Reactive.Linq;
using Serilog;

using Neurocache.Csharp.Nexus.Nodes.OpenAi;

namespace Neurocache.Csharp.Nexus.Nodes.NodeRouter
{
    public class NodeRouter
    {
        private static readonly NodeRouter instance = new();
        public static NodeRouter Instance => instance;
        private NodeRouter() { }

        public delegate Task<NodeRecord> NodeTask(string nodeId, string input);
        public static readonly ISubject<Bulletin> OnBulletinSubject
            = new Subject<Bulletin>();

        private static readonly Dictionary<string, NodeTask> nodes = new()
        {
            { "AvatarGen", AvatarGen.RunAsync },
            { "GptChat", GptChat.RunAsync },
            { "Persona", Persona.RunAsync },
        };

        public static void Init()
            => OnBulletinSubject
                .SelectMany(bulletin => Observable.FromAsync(() => ProcessBulletinAsync(bulletin)))
                .SelectMany(recs => recs)
                .Subscribe(rec =>
                {
                    SendToGateway(rec);
                });

        private static async Task<IEnumerable<NodeRecord>> ProcessBulletinAsync(Bulletin bulletin)
        {
            var tasks = new List<Task<NodeRecord>>();
            foreach (var nodeId in bulletin.NodeIds)
            {
                if (nodes.TryGetValue(nodeId, out var nodeTask))
                {
                    var task = nodeTask(nodeId, bulletin.Payload);
                    _ = task.ContinueWith(completedTask =>
                    {
                        SendToGateway(completedTask.Result);
                    });
                    tasks.Add(task);
                }
            }

            return await Task.WhenAll(tasks);
        }

        private static void SendToGateway(NodeRecord rec)
        {
            Log.Information($"Sending to gateway: {rec}");
        }
    }
}
