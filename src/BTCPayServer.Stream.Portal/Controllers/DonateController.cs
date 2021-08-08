using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Data.Enums;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Portal.Extensions;
using BTCPayServer.Stream.Portal.ViewModels.Donates;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using honzanoll.MVC.NetCore.ViewModels;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BTCPayServer.Stream.Portal.Controllers
{
    [AllowAnonymous]
    public class DonateController : Controller
    {
        #region Fields

        private readonly IBtcPayServerService btcPayServerService;
        private readonly IStreamlabsService streamlabsService;

        private readonly IUserRepository userRepository;

        #endregion

        #region Constructors

        public DonateController(
            IBtcPayServerService btcPayServerService,
            IStreamlabsService streamlabsService,
            IUserRepository userRepository)
        {
            this.btcPayServerService = btcPayServerService;
            this.streamlabsService = streamlabsService;

            this.userRepository = userRepository;
        }

        #endregion

        [HttpGet("Donate/{identifier}")]
        public async Task<IActionResult> Index(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return NotFound();

            ApplicationUser user = await userRepository.GetAsync(identifier);
            if (user == null)
                return NotFound();

            // Set default culture, if the user did not select the culture yet
            if (!Request.Cookies.ContainsKey(CookieRequestCultureProvider.DefaultCookieName))
            {
                Response.SetCultureCookie((user.DefaultCulture ?? Culture.EN).ToISO());
                return RedirectToAction(nameof(Index), new { identifier });
            }

            string problem = null;
            if (string.IsNullOrEmpty(user.BtcPayServerUri) ||
                string.IsNullOrEmpty(user.BtcPayServerStoreId) ||
                user.BtcPayServerAuthTokens.Count == 0)
                problem = CommonResource.Message_NoPaymentGateway;

            if (user.StylesheetFileObject != null)
                ViewData["StylesheetLink"] = "/donate.stylesheet?Identifier=" + user.DonatePageIdentifier;

            if (!string.IsNullOrEmpty(user.PageTitle))
                ViewData["PageTitle"] = user.PageTitle;

            if (user.LogoFileObject != null)
                ViewData["LogoLink"] = "/donate.logo?Identifier=" + user.DonatePageIdentifier;

            if (!string.IsNullOrEmpty(user.GtagId))
                ViewData["GtagId"] = user.GtagId;

            return View(new DonateFormViewModel
            {
                Currency = user.DefaultCurrency ?? InvoiceCurrency.USD,
                TargetUserId = user.Id,
                TargetUserIdentifier = user.DonatePageIdentifier,
                Problem = problem,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Donate(DonateFormViewModel values)
        {
            if (!ModelState.IsValid)
                return Json(new FormValidationsViewModel(ModelState));

            ApplicationUser applicationUser = await userRepository.GetAsync(values.TargetUserId);
            if (applicationUser == null)
                ModelState.AddModelError(nameof(values.TargetUserId), CommonResource.Validation_UnknownTargetUser);

            if (!ModelState.IsValid)
                return Json(new FormValidationsViewModel(ModelState));

            // Check message length by bytes count (Streamlabs required)
            string message = streamlabsService.PrepareMessage(values.Message);
            if (message != null && Encoding.UTF8.GetByteCount(message) > 230)
                ModelState.AddModelError(nameof(values.Message), CommonResource.Validation_MaxLength_230);

            // Check if tip value is enough
            ValidateTipValue(values.Amount.Value, values.Currency.Value, applicationUser);

            if (!ModelState.IsValid)
                return Json(new FormValidationsViewModel(ModelState));

            // Create new invoice
            Uri checkoutUrl = await btcPayServerService.CreateInvoiceAsync(values.Amount.Value, values.Donator, message, values.Currency.Value, applicationUser);

            // Redirect to invoice checkout
            return Json(new FormValidationsViewModel(
                new
                {
                    Currency = values.Currency.ToString(),
                    Value = values.Amount,
                    Url = checkoutUrl.AbsoluteUri
                }));
        }

        [HttpGet("Donate/Acknowledgment/{invoiceId}")]
        public IActionResult Acknowledgment(string invoiceId)
        {
            return View();
        }

        #region Private methods

        private void ValidateTipValue(decimal value, InvoiceCurrency currency, ApplicationUser applicationUser)
        {
            decimal? minValue = applicationUser.MinTipsObject?.SingleOrDefault(mt => mt.Currency == currency)?.MinValue;
            if (minValue.HasValue)
            {
                if (value < minValue.Value)
                    ModelState.AddModelError(nameof(DonateFormViewModel.Amount), string.Format(CommonResource.Validation_MinTip_Format, minValue.Value, currency));
            }
            else
            {
                if ((currency == InvoiceCurrency.USD || currency == InvoiceCurrency.EUR) && value < 1)
                    ModelState.AddModelError(nameof(DonateFormViewModel.Amount), string.Format(CommonResource.Validation_MinTip_Format, 1, currency));
                else if (currency == InvoiceCurrency.CZK && value < 20)
                    ModelState.AddModelError(nameof(DonateFormViewModel.Amount), string.Format(CommonResource.Validation_MinTip_Format, 20, currency));
                else if (currency == InvoiceCurrency.SAT && value < 3000)
                    ModelState.AddModelError(nameof(DonateFormViewModel.Amount), string.Format(CommonResource.Validation_MinTip_Format, 3000, currency));
            }
        }

        #endregion
    }
}
