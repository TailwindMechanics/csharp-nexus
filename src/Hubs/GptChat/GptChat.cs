//path: src\Hubs\GptChat\GptChat.cs

using System.Reactive.Subjects;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Hubs
{
    public static class GptChat
    {
        public static string HubAuthor => "gpt_chat";
        public static async void Run(OperationReport report, Action<string, OperationReport, string, bool> callback, CancellationToken cancelToken)
        {
            if (report.Recipient != HubAuthor) return;

            Ships.Log($"{HubAuthor} received report: {report}");

            await Task.Delay(TimeSpan.FromSeconds(10), cancelToken);
            callback.Invoke(HubAuthor, report,
                "demo: async fetch memories from pinecone",
                false
            );

            await Task.Delay(TimeSpan.FromSeconds(5), cancelToken);
            callback.Invoke(HubAuthor, report,
                "demo: async stream all message replies from gpt-4",
                false
            );

            await Task.Delay(TimeSpan.FromSeconds(5), cancelToken);
            callback.Invoke(HubAuthor, report,
                "demo: async update pinecone with new memories",
                true
            );
        }
    }
}
