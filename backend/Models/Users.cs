namespace ModelsUsers;

using System.Net.WebSockets;
using System.Collections.Concurrent;
using System.Text;

public static class Users
{
    private static ConcurrentBag<WebSocket> UsersPool = new ConcurrentBag<WebSocket>();

    public static ConcurrentBag<WebSocket> GetUsers()
    {
        return Users.UsersPool;
    }

    public static void RegisterNew(WebSocket webSocket)
    {
        Users.UsersPool.Add(webSocket);
    }

    public static async Task SendGlobalMessage(string msg)
    {
        foreach (WebSocket user_socket in Users.GetUsers())
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

