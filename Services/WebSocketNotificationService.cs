using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json; // Ensure Newtonsoft.Json is installed via NuGet for serialization

namespace SimpleBlogSystem.Services
{
    public class WebSocketNotificationService
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connectedClients = new();

        public async Task InitializeWebSocketAsync(HttpContext context, Func<Task> next)
        {
            if (!context.WebSockets.IsWebSocketRequest) return;

            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            string clientId = Guid.NewGuid().ToString();
            _connectedClients.TryAdd(clientId, webSocket);

            await ReceiveMessage(webSocket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    if (_connectedClients.TryRemove(clientId, out WebSocket? ws))
                    {
                        if (ws != null) 
                        {
                            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        }
                    }
                }
            });
        }

        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }

        public async Task NotifyNewPostAsync(dynamic post)
        {
            string message = JsonConvert.SerializeObject(post);
            await BroadcastMessageAsync(message);
        }

        private async Task BroadcastMessageAsync(string message)
        {
            foreach (var client in _connectedClients.Values)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message), 0, message.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
