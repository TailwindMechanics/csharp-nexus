//path: src\Operations\Operation.cs

using System.Reactive.Subjects;
using System.Reactive.Linq;

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
        readonly ConduitChannel conduitChannel;

        public Operation(Guid operationToken, Guid agentId)
        {
            var stopStream = OperationService.StopSubject
                .Where(token => token == operationToken)
                .Take(1);
            stopStream.Subscribe(_ => Stop());

            OperationToken = operationToken;
            conduitChannel = new ConduitChannel(agentId, operationToken);

            conduitChannel.OnReportReceived
                .Where(ValidConduitAuthor)
                .TakeUntil(stopStream)
                .Subscribe(OnReportReceived);

            uplinkHubReportSubject
                .TakeUntil(stopStream)
                .Where(ValidHubAuthor)
                .Subscribe(UplinkHubReport);

            conduitChannel.Start();

            Ships.Log($"Operation started with token {operationToken}");
        }

        void Stop()
        {
            cancelToken.Cancel();
            Ships.Log($"Operation stopped with token {OperationToken}");
        }

        void OnReportReceived(OperationReport report)
        {
            Ships.Log($"OnReportReceived: {report}");

            if (OnReceivedDispatchStopReport(report)) return;

            AvatarGen.Run(report, uplinkHubReportSubject, cancelToken.Token);
            GptChat.Run(report, uplinkHubReportSubject, cancelToken.Token);
            Persona.Run(report, uplinkHubReportSubject, cancelToken.Token);
        }

        bool OnReceivedDispatchStopReport(OperationReport report)
        {
            if (report.Author == "Vanguard" && report.ReportId == "vanguard_stop")
            {
                Ships.Log($"Dispatching stop signal for operation: {OperationToken}");
                OperationService.StopSubject.OnNext(OperationToken);
                return true;
            }

            return false;
        }

        void UplinkHubReport(OperationReport report)
        {
            Ships.Log($"Uplinking Hub Report: {report}");
            conduitChannel.SendReport.OnNext(report);
        }

        bool ValidConduitAuthor(OperationReport report)
            => report.Author == "Vanguard";

        bool ValidHubAuthor(OperationReport report)
            => report.Author == AvatarGen.HubAuthor
            || report.Author == GptChat.HubAuthor
            || report.Author == Persona.HubAuthor;
    }
}
