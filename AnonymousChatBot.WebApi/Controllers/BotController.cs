using AnonymousChatBot.Service.Implementations;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace AnonymousChatBot.WebApi.Controllers;

[ApiController]
[Route("bot")]
public class BotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update,
        [FromServices] HandleUpdateService handleUpdateService, CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);

        return Ok();
    }
}