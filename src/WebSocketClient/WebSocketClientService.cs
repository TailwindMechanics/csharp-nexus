//path: src\WebSocketClient\WebSocketClientService.cs

using System.Reactive.Subjects;
using System.Net.WebSockets;
using System.Reactive.Linq;

using Neurocache.ShipsInfo;

namespace Neurocache.WebSocketClient
{
    public class WebSocketClientService
    {
        public static WebSocketClientService Instance => instance;
        static readonly WebSocketClientService instance = new();

        public static readonly ISubject<string> SendMessageSubject = new Subject<string>();
        public static IObservable<string> ReceivedMessageStream => receivedMessageStream;
        readonly static Subject<string> receivedMessageStream = new();

        readonly CancellationTokenSource cancelToken;
        readonly ClientWebSocket clientWebSocket;

        IDisposable? sendMessageSub;
        IDisposable? connectionSub;
        IDisposable? dataSub;

        const double pollRateSeconds = 0.1;
        const double retryRateSeconds = 1;

        WebSocketClientService()
        {
            clientWebSocket = new ClientWebSocket();
            cancelToken = new CancellationTokenSource();
        }

        public void OnAppStarted()
        {
            var vanguardAddress = Ships.SocketAddress(Ships.VanguardStarship);
            Ships.Log($"WebSocketClientService: attempting to connect to {vanguardAddress}");

            var connectionStream = SocketStreams.ConnectionStream(
                clientWebSocket,
                vanguardAddress,
                retryRateSeconds
            );

            var stopStream = SocketStreams.StopStream(cancelToken.Token, connectionStream);
            connectionSub = connectionStream.Connect();

            dataSub = SocketStreams
                .DataStream(clientWebSocket, stopStream, pollRateSeconds)
                .Subscribe(data =>
                {
                    Ships.Log($"WebSocketClientService, received data: {data}");
                    receivedMessageStream.OnNext(data);
                });

            sendMessageSub = SendMessageSubject
                .Subscribe(async message =>
                {
                    Ships.Log($"WebSocketClientService, sending message: {message}");
                    await SocketStreams.SendMessageAsync(clientWebSocket, message);
                });
        }

        public void OnAppClosing()
        {
            Ships.Log("WebSocketClientService: onAppClosing");

            cancelToken.Cancel();

            Cleanup();
        }

        async void Cleanup()
        {
            if (clientWebSocket.State == WebSocketState.Open)
            {
                await clientWebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None
                );
            }

            sendMessageSub?.Dispose();
            connectionSub?.Dispose();
            dataSub?.Dispose();
            cancelToken.Dispose();
            clientWebSocket.Dispose();
        }
    }
}
