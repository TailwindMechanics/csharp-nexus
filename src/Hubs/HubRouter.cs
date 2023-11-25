//path: src\Hubs\HubRouter.cs

using Neurocache.Broadcasts;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public static class HubOperationService
    {
        static readonly Dictionary<Guid, HubOperation> operations = [];

        public static void Init()
        {
            BroadcastChannelService
                .StartHubOperationStream
                .Subscribe(operation =>
                {
                    operations[operation.OperationReport.Token] = operation;

                    GptChat.Run(operations[operation.OperationReport.Token]);
                    AvatarGen.Run(operations[operation.OperationReport.Token]);
                    Persona.Run(operations[operation.OperationReport.Token]);
                });
        }

        public static void Stop(Guid operationToken)
        {
            operations[operationToken].CancelToken.Cancel();
        }

        public static void Kill()
        {
            foreach (var hubOperation in operations.Values)
            {
                hubOperation.CancelToken.Cancel();
            }
        }
    }
}