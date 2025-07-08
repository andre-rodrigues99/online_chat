using System.Net.WebSockets;

namespace Models.ChatMessages;

public class ChatMessage
{
    public string message { get; set; } = "";


    public string EchoWs(WebSocket webSocket)
    {
        Console.WriteLine(webSocket);

        return "websocket working";
    }
}