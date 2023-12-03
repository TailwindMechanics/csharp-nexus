//path: src\Hubs\Persona\Persona.cs

using System.Reactive.Subjects;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class Persona
    {
        public static string HubAuthor => "persona";
        public static async void Run(OperationReport report, ISubject<OperationReport> callback, CancellationToken cancelToken)
        {
            Ships.Log($"{HubAuthor} received report: {report}");
            Ships.Log("Generating persona...");

            await Task.Delay(TimeSpan.FromSeconds(10), cancelToken);

            Ships.Log("Finished generating persona");

            callback.OnNext(new OperationReport(
                report.Token,
                HubAuthor,
                "Some persona",
                report.AgentId,
                true,
                report.ReportId
            ));
        }
    }
}
