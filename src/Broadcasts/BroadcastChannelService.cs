//path: src\Broadcasts\BroadcastChannelService.cs

using System.Reactive.Subjects;

using Neurocache.Schema;

namespace Neurocache.Broadcasts
{
    public class BroadcastChannelService
    {
        public static readonly ISubject<OperationReport> SendHubReportToVanguardStream = new Subject<OperationReport>();
        public static readonly ISubject<HubOperation> StartHubOperationStream = new Subject<HubOperation>();

        public static BroadcastChannelService Instance => instance;
        BroadcastChannelService() { }

        static readonly Dictionary<Guid, BroadcastChannel> broadcastChannels = [];
        static readonly BroadcastChannelService instance = new();

        public static BroadcastChannel OpenChannel(HttpRequest request, HttpResponse response, HttpContext context, Guid operationToken)
        {
            broadcastChannels[operationToken] = new BroadcastChannel(
                request,
                response,
                context,
                operationToken
            );

            return broadcastChannels[operationToken];
        }

        public static void Stop(Guid operationToken)
        {
            broadcastChannels[operationToken].Stop();
        }

        public static void Kill()
        {
            foreach (var channel in broadcastChannels.Values)
            {
                channel.Stop();
            }
        }
    }
}
