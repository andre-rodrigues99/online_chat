using System.Net.WebSockets;
using ModelsUsers;
using Models.ChatHandler;
using System.Text;

namespace Models.Connection;


public class Connection
{
    private StringBuilder accumulatedData = new StringBuilder();
    public async Task StartConnection(WebSocket webSocket)
    {
        Users.RegisterNew(new User("name", webSocket));

        await Channel(webSocket);
    }

    private async Task Channel(WebSocket webSocket)
    {
        Console.WriteLine($"Start Channel - SubProtocol: {webSocket.SubProtocol}");
        Console.WriteLine($"Start Channel - State: {webSocket.State}");

        var buffer = new byte[1024 * 4];        
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (webSocket.State == WebSocketState.Open)
        {
            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            if (receiveResult.Count > 0)
            {
                new Chat().ChatHandle(Encoding.UTF8.GetString(buffer, 0, receiveResult.Count), webSocket);
            }

        }

        if (receiveResult.CloseStatus.HasValue)
        {
            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
        else
        {
            await webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closed by server",
                CancellationToken.None);
        }
    }
}