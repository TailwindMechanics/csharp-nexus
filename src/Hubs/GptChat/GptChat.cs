//path: src\Hubs\GptChat\GptChat.cs

using System.Reactive.Subjects;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public static class GptChat
    {
        public static string HubAuthor => "gpt_chat";
        public static async void Run(OperationReport report, ISubject<OperationReport> callback, CancellationToken cancelToken)
        {
            if (report.Recipient != HubAuthor) return;

            Ships.Log($"{HubAuthor} received report: {report}");
            Ships.Log("demo: async fetch memories from pinecone");

            await Task.Delay(TimeSpan.FromSeconds(10), cancelToken);

            callback.OnNext(new OperationReport(
                report.Token,
                HubAuthor,
                report.Author,
                "demo: async fetch memories from pinecone",
                report.AgentId,
                false,
                report.ReportId
            ));

            await Task.Delay(TimeSpan.FromSeconds(5), cancelToken);

            Ships.Log("demo: async stream all message replies from gpt-4");

            callback.OnNext(new OperationReport(
                report.Token,
                HubAuthor,
                report.Author,
                "demo: async stream all message replies from gpt-4",
                report.AgentId,
                false,
                report.ReportId
            ));

            await Task.Delay(TimeSpan.FromSeconds(5), cancelToken);

            Ships.Log("demo: async update pinecone with new memories");

            callback.OnNext(new OperationReport(
                report.Token,
                HubAuthor,
                report.Author,
                "demo: async update pinecone with new memories",
                report.AgentId,
                true,
                report.ReportId
            ));
        }
    }
}
