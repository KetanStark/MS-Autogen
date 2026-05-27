using Microsoft.AspNetCore.Mvc;

using OpenRouterWebChat.Models;
using OpenRouterWebChat.Services;

namespace OpenRouterWebChat.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly OpenRouterService _service;

    public ChatController(OpenRouterService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] ChatRequest request)
    {
        var reply =
            await _service.AskAsync(request.Message);

        return Ok(new ChatResponse
        {
            Reply = reply
        });
    }
}