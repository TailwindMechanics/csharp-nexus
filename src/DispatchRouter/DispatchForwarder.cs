//path: src\DispatchRouter\DispatchForwarder.cs

using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive;

using Neurocache.Schema;
using Neurocache.Hubs;

namespace Neurocache.NodeRouter
{
    public class DispatchForwarder
    {
        private static readonly DispatchForwarder instance = new();
        public static DispatchForwarder Instance => instance;
        private DispatchForwarder() { }

        public static readonly ISubject<Unit> KillSubject
            = new Subject<Unit>();
        public static readonly ISubject<string> StopSubject
            = new Subject<string>();
        public static readonly ISubject<OperationReport> DispatchSubject
            = new Subject<OperationReport>();

        public static IObservable<OperationReport> ReportStream => reportSubject;
        static readonly ISubject<OperationReport> reportSubject = new Subject<OperationReport>();

        public static void Init()
        {
            DispatchSubject.Subscribe(OnDispatchReceived);
            StopSubject.Subscribe();
        }

        private static void OnDispatchReceived(OperationReport dispatch)
        {
            var hubOperation = new HubOperation(
                dispatch,
                reportSubject,
                StopSubject,
                KillSubject
            );

            AvatarGen.Run(hubOperation);
            GptChat.Run(hubOperation);
            Persona.Run(hubOperation);
        }
    }
}
