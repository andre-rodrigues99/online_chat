using System.Net.WebSockets;
using ModelsUsers;

namespace Models.Connection;

public class Connection
{
    public async Task StartConnection(WebSocket webSocket)
    {
        Users.RegisterNew(webSocket);
        
        await Channel(webSocket);
    }

    private async Task Channel(WebSocket webSocket)
    {
        Console.WriteLine($"Start Channel - SubProtocol: {webSocket.SubProtocol}"); 
        Console.WriteLine($"Start Channel - State: {webSocket.State}"); 

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
                await Users.SendGlobalMessage(msg);
            }
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}