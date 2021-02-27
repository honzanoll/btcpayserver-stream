using BTCPayServer.Stream.Portal.Controllers.Settings;
using Microsoft.AspNetCore.Mvc;

namespace BTCPayServer.Stream.Portal.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(ProfileController.Index), "Profile");
        }
    }
}
