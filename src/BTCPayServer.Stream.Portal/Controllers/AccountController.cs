using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Portal.ViewModels;
using BTCPayServer.Stream.Portal.ViewModels.Account;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using honzanoll.MVC.NetCore.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BTCPayServer.Stream.Portal.Controllers
{
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        #region Fields

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IUserRepository userRepository;

        #endregion

        #region Constructors

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

            this.userRepository = userRepository;
        }

        #endregion

        public IActionResult Login(string redirectUrl = null)
        {
            return View();
        }

        [HttpPost("account/login")]
        public async Task<IActionResult> ProcessLogin(LoginFormViewModel values, [FromServices] LinkGenerator linkGenerator, string redirectUrl = null)
        {
            if (ModelState.IsValid)
            {
                SignInResult signInResult = await signInManager.PasswordSignInAsync(values.Email, values.Password, isPersistent: false, lockoutOnFailure: false);

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(redirectUrl))
                        return Json(new FormValidationsViewModel(new Url(redirectUrl)));

                    return Json(new FormValidationsViewModel(linkGenerator, nameof(DashboardController.Index), "Dashboard"));
                }

                ModelState.AddModelError(nameof(values.Password), CommonResource.Validation_InvalidUsernameOrPassword);
            }

            return Json(new FormValidationsViewModel(ModelState));
        }

        public IActionResult Registration()
        {
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> FinishRegistration(GoogleRegisterFormViewModel values)
        {
            if (!ModelState.IsValid)
                return View(nameof(Registration), values);

            ExternalLoginInfo externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
                return BadRequest();

            values = GetFormViewModel(externalLoginInfo, values);

            if (userRepository.ExistsIdentifier(values.DonatePageIdentifier.ToLower()))
            {
                ModelState.AddModelError(nameof(values.DonatePageIdentifier), CommonResource.Validation_UsedIdentifier);
                return View(nameof(Registration), values);
            }

            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = values.Email,
                Email = values.Email,
                Firstame = values.Firstname,
                Surname = values.Surname,
                DonatePageIdentifier = values.DonatePageIdentifier.ToLower()
            };

            IdentityResult identityResult = await userManager.CreateAsync(applicationUser);
            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddLoginAsync(applicationUser, externalLoginInfo);
                if (identityResult.Succeeded)
                {
                    await signInManager.SignInAsync(applicationUser, isPersistent: false);

                    return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
                }
            }

            return View(nameof(ErrorController.Index), new ErrorViewModel(500, CommonResource.Message_CannotFinishUserRegistration));
        }

        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        #region Private methods

        private GoogleRegisterFormViewModel GetFormViewModel(ExternalLoginInfo externalLoginInfo, GoogleRegisterFormViewModel viewModel)
        {
            ClaimsIdentity identity = externalLoginInfo.Principal.Identities.FirstOrDefault();
            if (identity == null)
                return null;

            string email = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string firstname = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            string surname = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

            return new GoogleRegisterFormViewModel
            {
                Email = email,
                Firstname = firstname,
                Surname = surname,
                DonatePageIdentifier = viewModel?.DonatePageIdentifier
            };
        }

        #endregion
    }
}
