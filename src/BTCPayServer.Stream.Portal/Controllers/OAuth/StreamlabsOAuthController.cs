using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.Common.Models.Settings;
using BTCPayServer.Stream.Common.Models.Settings.HttpClients;
using BTCPayServer.Stream.Common.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using honzanoll.MVC.NetCore.Services.Abstractions;
using honzanoll.MVC.NetCore.ViewModels;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Portal.Controllers.OAuth
{
    [Route("oauth/streamlabs")]
    public class StreamlabsOAuthController : ControllerBase
    {
        #region Fields

        private readonly IStreamlabsService streamlabsService;

        private readonly IViewRendererService viewRendererService;

        private readonly GlobalSettings globalSettings;
        private readonly StreamlabsSettings streamlabsSettings;

        #endregion

        #region Constructors

        public StreamlabsOAuthController(
            IStreamlabsService streamlabsService,
            IViewRendererService viewRendererService,
            IOptions<GlobalSettings> globalSettingsOptions,
            IOptions<StreamlabsSettings> streamlabsSettingsOptions)
        {
            this.streamlabsService = streamlabsService;

            this.viewRendererService = viewRendererService;

            globalSettings = globalSettingsOptions.Value;
            streamlabsSettings = streamlabsSettingsOptions.Value;
        }

        #endregion

        [Route("login")]
        public IActionResult Login()
        {
            string url = streamlabsSettings.BaseApiUrl + streamlabsSettings.OAuthLoginUrl
                .Replace("##clientId##", streamlabsSettings.ClientId)
                .Replace("##redirectUri##", globalSettings.Infrastructure.FEUrl + "/oauth/streamlabs/callback");

            return Redirect(url);
        }

        [Route("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            await streamlabsService.ObtainAccessTokenAsync(code, CurrentUserId);

            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            return Json(new DialogViewModel(
                CommonResource.Title_StreamlabsConnection,
                await viewRendererService.RenderAsync<object>(nameof(Logout), null)));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> ProcessLogout()
        {
            await streamlabsService.ClearTokenAsync(CurrentUserId);

            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }
    }
}
