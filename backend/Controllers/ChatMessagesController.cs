using Microsoft.AspNetCore.Mvc;
using Models.ChatMessages;

namespace Controllers.ChatMessagesController;

[ApiController]
[Route("/api/chat")]
public class ChatMessagesController : ControllerBase
{
    [HttpGet("{msg}")]
    public ActionResult<ChatMessage> Get(string msg)
    {
        return new ChatMessage { message = msg };
    }
}