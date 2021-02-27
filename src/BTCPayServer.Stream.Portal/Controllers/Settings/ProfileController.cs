using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Portal.ViewModels.Settings;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using honzanoll.MVC.NetCore.ViewModels;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Portal.Controllers.Settings
{
    public class ProfileController : SettingsControllerBase
    {
        #region Fields

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;

        private readonly LinkGenerator linkGenerator;

        #endregion

        #region Constructors

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            LinkGenerator linkGenerator)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;

            this.linkGenerator = linkGenerator;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await userManager.GetUserAsync(User);

            return View(new ProfileFormViewModel
            {
                Email = currentUser.Email,
                DonatePageIdentifier = currentUser.DonatePageIdentifier
            });
        }

        [HttpPost]
        public async Task<IActionResult> Process(ProfileFormViewModel values)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(User);

                if (!userRepository.ExistsIdentifier(values.DonatePageIdentifier.ToLower(), CurrentUserId))
                {
                    currentUser.DonatePageIdentifier = values.DonatePageIdentifier;
                    await userRepository.UpdateAsync(currentUser);

                    return Json(new FormValidationsViewModel(linkGenerator, nameof(Index), "Settings"));
                }
                else
                    ModelState.AddModelError(nameof(values.DonatePageIdentifier), CommonResource.Validation_UsedIdentifier);
            }

            return Json(new FormValidationsViewModel(ModelState));
        }
    }
}
