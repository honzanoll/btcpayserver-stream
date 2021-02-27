using System.Security.Policy;
using System.Threading.Tasks;
using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Data.Models.Users;
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
            if (signInManager.IsSignedIn(User))
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");

            if (userRepository.AnyUser())
                return View();

            return RedirectToAction(nameof(Register));
        }

        [HttpPost("Account/Login")]
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

        public IActionResult Register()
        {
            if (userRepository.AnyUser())
                return RedirectToAction(nameof(Login));

            return View();
        }

        [HttpPost("Account/Register")]
        public async Task<IActionResult> FinishRegistration(RegisterFormViewModel values, [FromServices] LinkGenerator linkGenerator)
        {
            if (userRepository.AnyUser())
                return RedirectToAction(nameof(Login));

            if (ModelState.IsValid)
            {
                if (values.Password == values.ConfirmPassword)
                {
                    if (!userRepository.ExistsIdentifier(values.DonatePageIdentifier.ToLower()))
                    {
                        ApplicationUser applicationUser = new ApplicationUser
                        {
                            UserName = values.Email,
                            Email = values.Email,
                            DonatePageIdentifier = values.DonatePageIdentifier.ToLower()
                        };

                        IdentityResult identityResult = await userManager.CreateAsync(applicationUser, values.Password);
                        if (identityResult.Succeeded)
                        {
                            await signInManager.SignInAsync(applicationUser, isPersistent: false);

                            return Json(new FormValidationsViewModel(linkGenerator, nameof(DashboardController.Index), "Dashboard"));
                        }
                        else
                        {
                            foreach (IdentityError identityError in identityResult.Errors)
                            {
                                ModelState.AddModelError(nameof(values.Password), identityError.Description);
                            }
                        }
                    }
                    else
                        ModelState.AddModelError(nameof(values.DonatePageIdentifier), CommonResource.Validation_UsedIdentifier);
                }
                else
                    ModelState.AddModelError(nameof(values.ConfirmPassword), CommonResource.Validation_PasswordDontMatch);
            }

            return Json(new FormValidationsViewModel(ModelState));
        }

        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }
    }
}
