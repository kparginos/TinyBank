using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;

namespace TinyBank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : Controller
    {
        private readonly ICardService _card;
        public CardController(ICardService card)
        {
            _card = card;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterCardOptions options)
        {
            var result = await _card.RegisterAsync(options);

            return Json(result);
        }
    }
}
