using System.Net.WebSockets;
using System.Collections.Concurrent;
using Models.UsersHandle;

namespace Models.ConnectionHandler;

public class ConnectionHandler
{

    public async Task Register(WebSocket webSocket)
    {
        UsersHandler.AddUser(webSocket);
        
        await Channel(webSocket);
    }

    private async Task Channel(WebSocket webSocket)
    {
        Console.WriteLine($"Start Channel {webSocket}"); 

        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            if (receiveResult.Count > 0)
            {
                string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                await UsersHandler.SendGlobalMessage(msg);
            }
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}