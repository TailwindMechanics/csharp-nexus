//path: src\Hubs\Persona\Persona.cs

using System.Reactive.Subjects;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public class Persona
    {
        public static string HubAuthor => "persona";
        public static async void Run(OperationReport report, Action<string, OperationReport, string, bool> callback, CancellationToken cancelToken)
        {
            if (report.Recipient != HubAuthor) return;

            Ships.Log($"{HubAuthor} received report: {report}");

            await Task.Delay(TimeSpan.FromSeconds(10), cancelToken);
            callback.Invoke(HubAuthor, report, "Some persona", true);
        }
    }
}
