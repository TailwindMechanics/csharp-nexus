//path: src\Operations\OperationService.cs

using System.Collections.Concurrent;
using System.Reactive.Linq;
using Neurocache.Schema;

using Neurocache.RequestsChannel;
using Neurocache.ShipsInfo;

namespace Neurocache.Operations
{
    public static class OperationService
    {
        public static ConcurrentDictionary<Guid, Operation> Operations { get; } = new();

        public static void OnAppStarted()
        {
            RequestsChannelService
                .OnDownlinkReceived
                .Where(operationReport => operationReport.ReportId == "vanguard_started")
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

            var agentId = Guid.Parse(requestReport.Payload);
            CreateOperation(requestReport.Token, agentId);
        }

        public static void CreateOperation(Guid operationToken, Guid agentId)
        {
            var operation = new Operation(operationToken, agentId);
            Operations.TryAdd(operationToken, operation);
        }

        public static void Stop(Guid token)
        {
            Operations.TryGetValue(token, out var operation);
            if (operation == null)
            {
                Ships.Warning($"Operation with token {token} not found");
                return;
            }

            operation.Stop();
            Operations.TryRemove(token, out _);
        }

        public static void KillAll()
        {
            foreach (var operation in Operations.Values)
            {
                operation.Stop();
            }

            Operations.Clear();
        }
    }
}
