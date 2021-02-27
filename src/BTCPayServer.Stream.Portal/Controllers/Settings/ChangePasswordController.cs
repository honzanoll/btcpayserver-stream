using System.Threading.Tasks;
using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Portal.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using honzanoll.MVC.NetCore.ViewModels;

namespace BTCPayServer.Stream.Portal.Controllers.Settings
{
    public class ChangePasswordController : SettingsControllerBase
    {
        #region Fields

        private readonly UserManager<ApplicationUser> userManager;

        private readonly LinkGenerator linkGenerator;

        #endregion

        #region Constructors

        public ChangePasswordController(
            UserManager<ApplicationUser> userManager,
            LinkGenerator linkGenerator)
        {
            this.userManager = userManager;

            this.linkGenerator = linkGenerator;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Proces(ChangePasswordFormViewModel values)
        {
            if (ModelState.IsValid)
            {
                if (values.NewPassword == values.ConfirmPassword)
                {
                    ApplicationUser currentUser = await userManager.GetUserAsync(User);
                    IdentityResult identityResult = await userManager.ChangePasswordAsync(currentUser, values.CurrentPassword, values.NewPassword);
                    if (identityResult.Succeeded)
                        return Json(new FormValidationsViewModel(linkGenerator, nameof(Index), "Settings"));
                    else
                    {
                        foreach (IdentityError identityError in identityResult.Errors)
                        {
                            if (identityError.Code == "PasswordMismatch")
                                ModelState.AddModelError(nameof(values.CurrentPassword), identityError.Description);
                            else
                                ModelState.AddModelError(nameof(values.NewPassword), identityError.Description);
                        }
                    }
                }
                else
                    ModelState.AddModelError(nameof(values.ConfirmPassword), CommonResource.Validation_PasswordDontMatch);
            }

            return Json(new FormValidationsViewModel(ModelState));
        }
    }
}
