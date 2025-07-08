using Microsoft.AspNetCore.Mvc;
using Models.ConnectionHandler;

namespace Controllers.ChatMessagesControll;

public class ChatMessagesController : ControllerBase
{
    [Route("/ws_chat")]
    public async Task Get()
    {
        // aceite do ws
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            await new ConnectionHandler().StartConnection(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}