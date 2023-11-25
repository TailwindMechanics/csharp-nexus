//path: src\Hubs\Persona\Persona.cs

using System.Reactive.Linq;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class Persona : IHub
    {
        public static string HubId => nameof(Persona).ToLower();
        public static async void Run(HubOperation hubOperation)
            => await Hub.Run(HubId, hubOperation, ()
                => HubOperation(hubOperation));

        static async Task HubOperation(HubOperation hubOperation)
        {
            Ships.Log("Generating persona");

            await Task.Delay(TimeSpan.FromSeconds(10));

            hubOperation.Callback.OnNext(new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "Generating persona",
                true,
                []
            ));
        }
    }
}
