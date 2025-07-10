using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Models.Messages;
using ModelsUsers;

namespace Models.ChatHandler;

public class Chat
{
    public void ChatHandle(string msg_content, WebSocket webSocket)
    {
        int usr_id = Users.GetId(webSocket);
        int session_id = webSocket.GetHashCode();

        Message msg = new Message().OpenMessage(usr_id, session_id, msg_content);

        DeliverMessage(msg);
    }

    private async void DeliverMessage(Message msg)
    {
        bool has_access = true;

        if (has_access)
        {
            await SendMessage(msg);
        }
    }

    private static async Task SendMessage(Message msg)
    {
        var serialized_msg = new ClientMessage().FromMessage(msg);

        foreach (var value in Users.GetGroup(msg.msg_to))
        {
            User user = value.Value;

            if (user.user_conn.GetHashCode() == msg.session_id)
            {
                continue;
            }


            if (user.user_conn.State == WebSocketState.Open)
            {
                await user.user_conn.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(serialized_msg))),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                    );
            }

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