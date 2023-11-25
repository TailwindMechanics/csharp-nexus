//path: src\DispatchRouter\DispatchForwarder.cs

using System.Reactive.Subjects;

using Neurocache.ShipsInfo;
using Neurocache.Schema;
using Neurocache.Hubs;

namespace Neurocache.NodeRouter
{
    public class DispatchForwarder
    {
        static readonly Dictionary<string, CancellationTokenSource> operations = [];
        private static readonly DispatchForwarder instance = new();
        public static DispatchForwarder Instance => instance;
        private DispatchForwarder() { }

        public static IObservable<OperationReport> ReportStream => reportSubject;
        static readonly ISubject<OperationReport> reportSubject = new Subject<OperationReport>();

        public static void Stop(string operationToken)
        {
            Ships.Log($"DispatchForwarder/Stop: operations[operationToken]: {operations[operationToken]}");
            operations[operationToken].Cancel();
        }

        public static void Kill()
        {
            Ships.Log($"DispatchForwarder/Kill: operations: {operations.Count}");
            foreach (var cancelToken in operations.Values)
            {
                cancelToken.Cancel();
            }
        }

        public static void Dispatch(OperationReport dispatch, CancellationToken httpCancel)
        {
            var cancelToken = CancellationTokenSource.CreateLinkedTokenSource(httpCancel);

            Ships.Log($"DispatchForwarder/Dispatch: cancelToken: {cancelToken}");

            var hubOperation = new HubOperation(
                dispatch,
                reportSubject,
                cancelToken
            );

            Ships.Log($"DispatchForwarder/Dispatch: hubOperation: {hubOperation}");

            Ships.Log($"DispatchForwarder/Dispatch: operations: {operations}");

            operations[dispatch.Token] = cancelToken;

            Ships.Log($"DispatchForwarder/Dispatch: operations[dispatch.Token]: {operations[dispatch.Token]}");

            AvatarGen.Run(hubOperation);
            GptChat.Run(hubOperation);
            Persona.Run(hubOperation);
        }
    }
}
