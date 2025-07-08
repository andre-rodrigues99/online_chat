using Models.Connection;

namespace Controllers.ChatApi;

public class ChatApiController : ControllerBase
{
    [Route("/ws_chat")]
    public async Task Get()
    {
        // aceite do ws
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            await new Connection().StartConnection(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}