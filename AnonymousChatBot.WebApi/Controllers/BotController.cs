using AnonymousChatBot.Service.Implementations;
using AnonymousChatBot.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace AnonymousChatBot.WebApi.Controllers;

[ApiController]
[Route("bot")]
public class BotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update,
        [FromServices] HandleUpdate handleUpdate, CancellationToken cancellationToken)
    {
        await handleUpdate.HandleUpdateAsync(update, cancellationToken);

        return Ok();
    }
}