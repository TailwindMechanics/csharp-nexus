//path: src\Operations\OperationService.cs

using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Reactive.Linq;

using Neurocache.RequestsChannel;
using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Operations
{
    public static class OperationService
    {
        public static readonly Subject<Guid> StopSubject = new();
        public static ConcurrentDictionary<Guid, Operation> Operations { get; } = new();

        public static void OnAppStarted()
        {
            var stopStream = StopSubject.Subscribe(Stop);

            RequestsChannelService
                .OnDownlinkReceived
                .Subscribe(OnOperationRequestReceived);

            Ships.Log("OperationService/OnAppStarted");
        }

        public static void OnAppClosing()
        {
            Ships.Log("OperationService/OnAppClosing");
        }

        public static void OnOperationRequestReceived(OperationReport requestReport)
        {
            Ships.Log($"OperationService/OnOperationRequestReceived: {requestReport}");

            if (requestReport.ReportId == "vanguard_started")
            {
                Ships.Log($"OperationService/OnOperationRequestReceived: vanguard_started");
                CreateOperation(requestReport.Token, requestReport.AgentId);
            }
        }

        public static void CreateOperation(Guid operationToken, Guid agentId)
        {
            Ships.Log($"OperationService/CreateOperation: {operationToken}");
            var operation = new Operation(operationToken, agentId);
            Operations.TryAdd(operationToken, operation);
        }

        static void Stop(Guid token)
        {
            Operations.TryGetValue(token, out var operation);
            if (operation == null)
            {
                Ships.Warning($"Operation with token {token} not found");
                return;
            }

            Operations.TryRemove(token, out _);
        }

        public static void KillAll()
        {
            foreach (var operation in Operations.Values)
            {
                Stop(operation.OperationToken);
            }

            Operations.Clear();
        }
    }
}
