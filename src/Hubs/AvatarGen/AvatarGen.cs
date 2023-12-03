//path: src\Hubs\AvatarGen\AvatarGen.cs

using System.Reactive.Subjects;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class AvatarGen
    {
        public static string HubAuthor => "avatar_gen";
        public static async void Run(OperationReport report, ISubject<OperationReport> callback, CancellationToken cancelToken)
        {
            if (report.Recipient != HubAuthor) return;

            Ships.Log($"{HubAuthor} received report: {report}");
            Ships.Log("Generating avatar...");

            await Task.Delay(TimeSpan.FromSeconds(1), cancelToken);

            Ships.Log("Finished generating avatar");
            callback.OnNext(new OperationReport(
                report.Token,
                HubAuthor,
                report.Author,
                "Generating avatar",
                report.AgentId,
                true,
                report.ReportId
            ));
        }
    }
}
