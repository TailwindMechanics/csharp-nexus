//path: src\Hubs\AvatarGen\AvatarGen.cs

using System.Reactive.Subjects;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class AvatarGen
    {
        public static string HubTypeId => "avatar_gen";
        public static async void Run(OperationReport report, ISubject<OperationReport> callback, CancellationToken cancelToken)
        {
            Ships.Log($"{HubTypeId} received report: {report}");
            Ships.Log("Generating avatar...");

            await Task.Delay(TimeSpan.FromSeconds(1), cancelToken);

            Ships.Log("Finished generating avatar");
            callback.OnNext(new OperationReport(
                report.Token,
                HubTypeId,
                "Generating avatar",
                true,
                report.ReportId
            ));
        }
    }
}
