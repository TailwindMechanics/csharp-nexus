//path: src\Operations\Operation.cs

using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive;

using Neurocache.ShipsInfo;
using Neurocache.Schema;
using Neurocache.Hubs;

namespace Neurocache.Operations
{
    public class Operation
    {
        public readonly Guid OperationToken;
        readonly ISubject<OperationReport> uplinkHubReportSubject = new Subject<OperationReport>();
        readonly CancellationTokenSource cancelToken = new();
        readonly ISubject<Unit> stop = new Subject<Unit>();
        readonly ConduitChannel conduitChannel;

        public Operation(Guid operationToken, Guid agentId)
        {
            OperationToken = operationToken;
            conduitChannel = new ConduitChannel(agentId);

            conduitChannel.OnReportReceived
                .Where(ValidConduitAuthor)
                .TakeUntil(stop)
                .Subscribe(OnReportReceived);

            uplinkHubReportSubject
                .TakeUntil(stop)
                .Where(ValidHubAuthor)
                .Subscribe(UplinkHubReport);

            conduitChannel.Start();

            Ships.Log($"Operation started with token {operationToken}");
        }

        public void Stop()
        {
            Ships.Log($"Stopping operation with token {OperationToken}");
            cancelToken.Cancel();
            conduitChannel.Stop();
            stop.OnNext(Unit.Default);
        }

        void OnReportReceived(OperationReport report)
        {
            Ships.Log($"OnReportReceived: {report}");

            AvatarGen.Run(report, uplinkHubReportSubject, cancelToken.Token);
            GptChat.Run(report, uplinkHubReportSubject, cancelToken.Token);
            Persona.Run(report, uplinkHubReportSubject, cancelToken.Token);
        }

        void UplinkHubReport(OperationReport report)
        {
            Ships.Log($"Uplinking Hub Report: {report}");
            conduitChannel.SendReport.OnNext(report);
        }

        bool ValidConduitAuthor(OperationReport report)
            => report.Author == "Vanguard";

        bool ValidHubAuthor(OperationReport report)
            => report.Author == AvatarGen.HubTypeId
            || report.Author == GptChat.HubTypeId
            || report.Author == Persona.HubTypeId;
    }
}
