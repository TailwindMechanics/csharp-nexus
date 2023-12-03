//path: src\Operations\ConduitChannel.cs

using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Reactive.Linq;

using Neurocache.ConduitFrigate;
using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Operations
{
    public class ConduitChannel(Guid agentid, Guid operationToken)
    {
        public readonly ISubject<OperationReport> SendReport = new Subject<OperationReport>();
        readonly Subject<OperationReport> onReportReceived = new();
        public IObservable<OperationReport> OnReportReceived
            => onReportReceived;

        IDisposable? channelClosedSub;
        IDisposable? downlinkSub;
        IDisposable? uplinkSub;

        public void Start()
        {
            var stopStream = OperationService.StopSubject
                .Where(token => token == operationToken)
                .Take(1);
            stopStream.Subscribe(_ => Stop());

            channelClosedSub = Conduit.ChannelClosed
                .Take(1)
                .TakeUntil(stopStream)
                .Subscribe(_ => OperationService.StopSubject.OnNext(operationToken));

            uplinkSub = SendReport
                .ObserveOn(Scheduler.Default)
                .TakeUntil(stopStream)
                .Subscribe(operationReport =>
                {
                    Ships.Log($"ConduitChannel: Sending operation report: {operationReport}");
                    Conduit.Uplink(agentid.ToString(), operationReport, CancellationToken.None);
                });

            downlinkSub = Conduit.Downlink(agentid.ToString(), Conduit.DownlinkConsumer, CancellationToken.None)
                .ObserveOn(Scheduler.Default)
                .Where(report => report != null)
                .TakeUntil(stopStream)
                .Subscribe(operationReport =>
                {
                    Ships.Log($"ConduitChannel: Received operation report: {operationReport}");
                    onReportReceived.OnNext(operationReport!);
                });

            Ships.Log("ConduitChannel: Started");
        }

        void Stop()
        {
            channelClosedSub?.Dispose();
            downlinkSub?.Dispose();
            uplinkSub?.Dispose();

            Ships.Log("ConduitChannel: Stopped");
        }
    }
}
