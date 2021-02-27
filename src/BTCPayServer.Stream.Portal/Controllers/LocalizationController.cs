using BTCPayServer.Stream.Portal.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BTCPayServer.Stream.Portal.Controllers
{
    public class LocalizationController : Controller
    {
        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            Response.SetCultureCookie(culture);

            return Ok();
        }
    }
}
