namespace Models.UsersHandle;

using System.Net.WebSockets;
using System.Collections.Concurrent;
using System.Text;

public static class UsersHandler
{
    private static ConcurrentBag<WebSocket> Users = new ConcurrentBag<WebSocket>();

    public static ConcurrentBag<WebSocket> GetUsers()
    {
        return UsersHandler.Users;
    }

    public static void AddUser(WebSocket webSocket)
    {
        UsersHandler.Users.Add(webSocket);
    }

    public static async Task SendGlobalMessage(string msg)
    {
        foreach (WebSocket user_socket in UsersHandler.GetUsers())
        {
            await user_socket.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }
}

