using System.Net.WebSockets;
using ModelsUsers;
using Models.ChatHandler;
using Models.Messages;

namespace Models.Connection;

public class Connection
{
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
                Console.WriteLine(webSocket.GetHashCode());

                int id = Users.GetId(webSocket);
                Message msg = new Message(id, System.Text.Encoding.UTF8.GetString(buffer, 0, receiveResult.Count));
                new Chat().ChatHandle(msg);
            }
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}