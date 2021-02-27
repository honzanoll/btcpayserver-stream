using System;
using System.Threading.Tasks;
using BTCPayServer.Stream.Business.Caches.Abstractions;
using BTCPayServer.Stream.Business.Models.BtcPayServer;
using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.Common.Models.Settings;
using BTCPayServer.Stream.Common.Models.Settings.HttpClients;
using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Portal.ViewModels.BtcPayServer;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using honzanoll.MVC.NetCore.Services.Abstractions;
using honzanoll.MVC.NetCore.ViewModels;

namespace BTCPayServer.Stream.Portal.Controllers.OAuth
{
    [Route("oauth/btcpayserver")]
    public class BtcPayServerOAuthController : ControllerBase
    {
        #region Fields

        private readonly ICache<OAuthAuthorizedData> cache;
        private readonly IBtcPayServerService btcPayServerService;

        private readonly IViewRendererService viewRendererService;

        private readonly IUserRepository userRepository;

        private readonly GlobalSettings globalSettings;
        private readonly BtcPayServerSettings btcPayServerSettings;

        private ILogger<BtcPayServerOAuthController> logger;

        #endregion

        #region Constructors

        public BtcPayServerOAuthController(
            ICache<OAuthAuthorizedData> cache,
            IBtcPayServerService btcPayServerService,
            IViewRendererService viewRendererService,
            IUserRepository userRepository,
            IOptions<GlobalSettings> globalSettingsOptions,
            IOptions<BtcPayServerSettings> btcPayServerSettingsOptions,
            ILogger<BtcPayServerOAuthController> logger)
        {
            this.cache = cache;
            this.btcPayServerService = btcPayServerService;

            this.viewRendererService = viewRendererService;

            this.userRepository = userRepository;

            globalSettings = globalSettingsOptions.Value;
            btcPayServerSettings = btcPayServerSettingsOptions.Value;

            this.logger = logger;
        }

        #endregion

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            (string serverUrl, string storeId) = await userRepository.GetBtcPayServerDataAsync(CurrentUserId);

            return Json(new DialogViewModel(
                CommonResource.Title_BTCPayServerConnection,
                await viewRendererService.RenderAsync(nameof(Login), new BtcPayServerLoginViewModel()
                {
                    ServerUrl = serverUrl,
                    StoreId = storeId
                })));
        }

        [HttpPost("login")]
        public async Task<IActionResult> ProcessLogin(BtcPayServerLoginViewModel values)
        {
            if (!ModelState.IsValid)
                return Json(new FormValidationsViewModel(ModelState));

            Uri baseUrl = new Uri(values.ServerUrl);

            string url = btcPayServerSettings.OAuthLoginUrl
                .Replace("##permissions##", "btcpay.store.webhooks.canmodifywebhooks&permissions=btcpay.store.cancreateinvoice&permissions=btcpay.store.canviewinvoices")
                .Replace("##applicationName##", "BTCPay-Streamlabs system")
                .Replace("##selectiveStores##", "true")
                .Replace("##redirect##", globalSettings.Infrastructure.FEUrl + "/oauth/btcpayserver/authorize")
                .Replace("##applicationIdentifier##", "BTCPayServer.Stream");

            await userRepository.StoreBtcPayServerAsync(CurrentUserId, baseUrl.AbsoluteUri, values.StoreId);

            return Json(new FormValidationsViewModel(new Uri(baseUrl, url)));
        }

        [HttpPost("authorize")]
        [AllowAnonymous]
        public IActionResult Callback(OAuthAuthorizedData data)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Guid correlationId = cache.Add(data);

            Response.Cookies.Append("BTCPayServer.Stream.Correlation", correlationId.ToString(), new CookieOptions { HttpOnly = true });
            return RedirectToAction(nameof(BtcPayServerOAuthController.InitConnection));
        }

        public async Task<IActionResult> InitConnection()
        {
            if (!Request.Cookies.TryGetValue("BTCPayServer.Stream.Correlation", out string correlation) ||
                !Guid.TryParse(correlation, out Guid correlationId) ||
                !cache.TryGet(correlationId, out OAuthAuthorizedData data))
            {
                logger.LogWarning("Valid correlationId not found");
                return BadRequest();
            }

            await btcPayServerService.InitConnectionAsync(data.ApiKey, CurrentUserId);

            Response.Cookies.Delete("BTCPayServer.Stream.Correlation");
            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            return Json(new DialogViewModel(
                CommonResource.Title_BTCPayServerConnection,
                await viewRendererService.RenderAsync<object>(nameof(Logout), null)));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> ProcessLogout()
        {
            await btcPayServerService.ClearConnectionAsync(CurrentUserId);

            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }
    }
}
