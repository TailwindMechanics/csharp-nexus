//path: src\WebSocketClient\SocketStreams.cs

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;

using Neurocache.ShipsInfo;

namespace Neurocache.WebSocketClient
{
    public static class SocketStreams
    {
        public static IConnectableObservable<WebSocketState> ConnectionStream(ClientWebSocket socket, string address, double retryRateSeconds)
        {
            return Observable.Interval(TimeSpan.FromSeconds(retryRateSeconds))
                .StartWith(0L)
                .SelectMany(async _ =>
                {
                    if (socket.State == WebSocketState.None || socket.State == WebSocketState.Closed || socket.State == WebSocketState.Aborted)
                    {
                        await socket.ConnectAsync(new Uri(address), CancellationToken.None);
                        Ships.Log($"ConnectionStream: successfully connected to {address}");
                    }

                    return socket.State;
                })
                .Publish();
        }

        public static IObservable<Unit> StopStream(CancellationToken cancellationToken, IObservable<WebSocketState> connectionStream)
        {
            return connectionStream
                .Where(state => state == WebSocketState.Closed || state == WebSocketState.Aborted)
                .Select(_ => Unit.Default)
                .Merge(CancelStream(cancellationToken))
                .Take(1);
        }

        public static IObservable<string> DataStream(ClientWebSocket socket, IObservable<Unit> stopStream, double pollRateSeconds)
        {
            return Observable.Interval(TimeSpan.FromSeconds(pollRateSeconds))
                .Where(_ => socket.State == WebSocketState.Open)
                .SelectMany(async _ =>
                {
                    var buffer = new byte[1024];
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    return Encoding.UTF8.GetString(buffer, 0, result.Count);
                })
                .TakeUntil(stopStream);
        }

        public static IObservable<Unit> CancelStream(CancellationToken token)
            => Observable.Create<Unit>(o =>
            {
                var registration = token.Register(() => o.OnNext(Unit.Default));
                return Disposable.Create(() => registration.Dispose());
            });

        public static async Task SendMessageAsync(ClientWebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
            {
                Ships.Log("SendMessageAsync: Cannot send message, WebSocket is not open");
                return;
            }

            var buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }
}