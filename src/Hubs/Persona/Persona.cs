//path: src\Hubs\Persona\Persona.cs

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class Persona : IHub
    {
        public static string HubId => nameof(Persona).ToLower();
        public static async void Run(HubOperation hubOperation)
        {
            Ships.Log("Generating persona");

            await Wait(10, hubOperation);

            hubOperation.Callback.OnNext(new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "Generating persona",
                true,
                []
            ));
        }

        static Task Wait(float time, HubOperation hubOperation)
            => Task.Delay(TimeSpan.FromSeconds(time),
                hubOperation.CancelToken.Token);
    }
}
