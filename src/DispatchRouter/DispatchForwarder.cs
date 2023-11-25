//path: src\DispatchRouter\DispatchForwarder.cs

using System.Reactive.Subjects;
using Neurocache.ShipsInfo;
using Neurocache.Schema;
using Neurocache.Hubs;

namespace Neurocache.NodeRouter
{
    public class DispatchForwarder
    {
        static readonly Dictionary<string, CancellationTokenSource> operations = new Dictionary<string, CancellationTokenSource>();
        private static readonly DispatchForwarder instance = new();
        public static DispatchForwarder Instance => instance;
        private DispatchForwarder() { }

        public static IObservable<OperationReport> ReportStream => reportSubject;
        static readonly ISubject<OperationReport> reportSubject = new Subject<OperationReport>();

        public static void InitializeDispatch(string token, CancellationToken httpCancel)
        {
            if (!operations.ContainsKey(token))
            {
                var cancelToken = CancellationTokenSource.CreateLinkedTokenSource(httpCancel);
                operations[token] = cancelToken;
            }
        }

        public static void Stop(string operationToken)
        {
            if (operations.TryGetValue(operationToken, out var cancelToken))
            {
                cancelToken.Cancel();
                cancelToken.Dispose();
                operations.Remove(operationToken);
                Ships.Log($"Stopped operation: {operationToken}");
            }
        }

        public static void Kill()
        {
            foreach (var kvp in operations)
            {
                kvp.Value.Cancel();
                kvp.Value.Dispose();
            }
            operations.Clear();
            Ships.Log("Killed all operations");
        }

        public static void Dispatch(OperationReport dispatch, CancellationToken httpCancel)
        {
            var cancelToken = CancellationTokenSource.CreateLinkedTokenSource(httpCancel);
            operations[dispatch.Token] = cancelToken;

            var hubOperation = new HubOperation(dispatch, reportSubject, cancelToken);
            AvatarGen.Run(hubOperation);
            GptChat.Run(hubOperation);
            Persona.Run(hubOperation);

            Ships.Log($"Dispatched operation: {dispatch.Token}");
        }
    }
}
