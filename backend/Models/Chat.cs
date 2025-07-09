using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using Models.Messages;
using ModelsUsers;

namespace Models.ChatHandler;

public class Chat
{
    public void ChatHandle(Message msg)
    {
        DeliverMessage(0, msg);
    }

    private async void DeliverMessage(int group, Message msg)
    {
        if (group == 0)
        {
            await SendGlobalMessage(msg.message);
        }
    }

    private static async Task SendGlobalMessage(string msg)
    {
        foreach (var value in Users.GetUsers())
        {
            User user = value.Value;

            if (user.user_conn.State == WebSocketState.Open)
            {
                await user.user_conn.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                    );
            }

        }
    }

}