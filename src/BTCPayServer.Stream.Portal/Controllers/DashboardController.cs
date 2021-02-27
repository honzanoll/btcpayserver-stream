using System.Threading.Tasks;
using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Business.Models.Streamlabs;
using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.Common.Models.Settings;
using BTCPayServer.Stream.Data.Enums;
using BTCPayServer.Stream.Data.Models.OAuth;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Portal.ViewModels.Dashboard;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using honzanoll.Repository.Abstractions;

namespace BTCPayServer.Stream.Portal.Controllers
{
    public class DashboardController : ControllerBase
    {
        #region Fields

        private readonly IStreamlabsService streamlabsService;
        private readonly IPriceStore priceStore;

        private readonly IRepository<StreamlabsAuthToken> streamlabsAuthTokenRepository;
        private readonly IRepository<BtcPayServerAuthToken> btcPayServerAuthTokenRepository;
        private readonly IUserRepository userRepository;

        private readonly GlobalSettings globalSettings;

        #endregion

        #region Constructors

        public DashboardController(
            IStreamlabsService streamlabsService,
            IPriceStore priceStore,
            IRepository<StreamlabsAuthToken> streamlabsAuthTokenRepository,
            IRepository<BtcPayServerAuthToken> btcPayServerAuthTokenRepository,
            IUserRepository userRepository,
            IOptions<GlobalSettings> globalSettingsOptions)
        {
            this.streamlabsService = streamlabsService;
            this.priceStore = priceStore;

            this.streamlabsAuthTokenRepository = streamlabsAuthTokenRepository;
            this.btcPayServerAuthTokenRepository = btcPayServerAuthTokenRepository;

            this.userRepository = userRepository;

            globalSettings = globalSettingsOptions.Value;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel();

            StreamlabsAuthToken streamlabsAuthToken = await streamlabsAuthTokenRepository.GetAsync(sat => sat.UserId == CurrentUserId);
            viewModel.IsStreamlabsConnected = streamlabsAuthToken != null;

            BtcPayServerAuthToken btcPayServerAuthToken = await btcPayServerAuthTokenRepository.GetAsync(sat => sat.UserId == CurrentUserId);
            viewModel.IsBtcPayServerConnected = btcPayServerAuthToken != null;

            viewModel.BitcoinCurrentPrice = priceStore.GetPrice(Currency.USD);

            ApplicationUser user = await userRepository.GetAsync(CurrentUserId);
            if (user != null)
            {
                viewModel.DonationUrl = globalSettings.Infrastructure.FEUrl + "/donate/" + user.DonatePageIdentifier;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TestDonation()
        {
            double currentRate = priceStore.GetPrice(Currency.USD);
            string value = (15 / currentRate * 100000000).ToString("0") + " SAT";

            await streamlabsService.SendDonateAsync(CurrentUserId, new Donate
            {
                Name = "Satoshi Nakamoto",
                Message = $"[TEST] Satoshi loves you ;-) {value}",
                Identifier = "me@honzanoll.cz",
                Amount = 15.0,
                Currency = InvoiceCurrency.USD
            });

            return RedirectToAction(nameof(Index));
        }
    }
}
