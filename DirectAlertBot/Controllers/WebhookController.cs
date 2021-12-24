using DirectAlertBot.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DirectAlertBot.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] CommandExecutorService commandExecutorService, [FromBody] Update update)
        {
            await commandExecutorService.ExecuteAsync(update);
            return Ok();
        }
    }
}
