//path: src\DispatchRouter\DispatchForwarder.cs

using System.Reactive.Subjects;

using Neurocache.Schema;
using Neurocache.Hubs;

namespace Neurocache.NodeRouter
{
    public class DispatchForwarder
    {
        private static readonly DispatchForwarder instance = new();
        public static DispatchForwarder Instance => instance;
        private DispatchForwarder() { }

        public static IObservable<OperationReport> ReportStream => reportSubject;
        static readonly ISubject<OperationReport> reportSubject = new Subject<OperationReport>();

        static readonly Dictionary<string, CancellationTokenSource> operations = [];

        public static void Stop(string operationToken)
        {
            operations[operationToken].Cancel();
        }

        public static void Kill()
        {
            foreach (var cancelToken in operations.Values)
            {
                cancelToken.Cancel();
            }
        }

        public static void Dispatch(OperationReport dispatch, CancellationToken httpCancel)
        {
            var cancelToken = CancellationTokenSource.CreateLinkedTokenSource(httpCancel);

            var hubOperation = new HubOperation(
                dispatch,
                reportSubject,
                cancelToken
            );

            operations[dispatch.Token] = cancelToken;

            AvatarGen.Run(hubOperation);
            GptChat.Run(hubOperation);
            Persona.Run(hubOperation);
        }
    }
}
