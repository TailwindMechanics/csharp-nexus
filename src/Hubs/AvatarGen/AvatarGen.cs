//path: src\Hubs\AvatarGen\AvatarGen.cs

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class AvatarGen : IHub
    {
        public static string HubId => nameof(AvatarGen).ToLower();
        public static async void Run(HubOperation hubOperation)
        {
            await Wait(1, hubOperation);

            Ships.Log("Generating avatar");

            hubOperation.Callback.OnNext(new OperationReport(
                hubOperation.OperationReport.Token,
                HubId,
                "Generating avatar",
                true,
                []
            ));
        }

        static Task Wait(float time, HubOperation hubOperation)
            => Task.Delay(TimeSpan.FromSeconds(time),
                hubOperation.CancelToken.Token);
    }
}
