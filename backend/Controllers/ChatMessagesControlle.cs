using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Models.ChatMessages;

namespace Controllers.ChatMessagesControll;

public class ChatMessagesController : ControllerBase
{
    [Route("/chat")]
    public async Task Get()
    {
        // aceite de ws obrigatório https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/websockets?view=aspnetcore-9.0
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var msg_handler = new ChatMessage();
            msg_handler.EchoWs(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}