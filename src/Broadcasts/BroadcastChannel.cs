//path: src\Broadcasts\BroadcastChannel.cs

using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Text;

using Neurocache.ShipsInfo;
using Neurocache.Schema;

namespace Neurocache.Broadcasts
{
    public class BroadcastChannel
    {
        readonly CancellationTokenSource cancelToken;
        public readonly Task ReadingTask;
        public readonly Task WritingTask;
        readonly Guid operationToken;

        public BroadcastChannel(HttpRequest request, HttpResponse response, HttpContext context, Guid operationToken)
        {
            this.operationToken = operationToken;

            var stream = response.Body;
            cancelToken = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);
            ReadingTask = ReadIncomingReports(request);
            WritingTask = WriteOutgoingReports(stream);
        }

        public void Stop()
        {
            cancelToken.Cancel();
        }

        Task WriteOutgoingReports(Stream stream)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            BroadcastChannelService
                .StartHubOperationStream
                .Subscribe(
                async report =>
                {
                    Ships.Log($"Sending report to Vanguard: {report}");
                    var reportJson = JsonConvert.SerializeObject(report);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(reportJson), cancelToken.Token);
                    await stream.FlushAsync(cancelToken.Token);
                },
                onError: ex => taskCompletionSource.TrySetException(ex),
                onCompleted: () => taskCompletionSource.TrySetResult(true));

            return taskCompletionSource.Task;
        }

        Task ReadIncomingReports(HttpRequest request)
        {
            return Task.Run(async () =>
            {
                using var reader = new StreamReader(request.Body);
                while (!cancelToken.IsCancellationRequested)
                {
                    var report = await RelevantReport(reader);
                    if (report == null) continue;

                    var hubOperation = new HubOperation(report, cancelToken);
                    BroadcastChannelService.StartHubOperationStream.OnNext(hubOperation);
                }
            }, cancelToken.Token);
        }

        async Task<OperationReport?> RelevantReport(StreamReader reader)
        {
            var incomingData = await reader.ReadLineAsync();
            if (incomingData == null) return null;

            var report = JsonConvert.DeserializeObject<OperationReport>(incomingData);
            var irrelevant = report == null || report.Token != operationToken || report.Author != Ships.VanguardName;
            return irrelevant ? null : report;
        }
    }
}