using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.CoinGeckoClient.Models.Responses;
using Microsoft.Extensions.Logging;
using honzanoll.Common.Workers.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Business.Services
{
    public class PriceLoaderHostedService : PeriodicalTaskHostedService
    {
        #region Fields

        private readonly IPriceStore priceStore;

        private readonly ICoinGeckoHttpClient coinGeckoHttpClient;

        private readonly ILogger<PriceLoaderHostedService> logger;

        #endregion

        #region Protected members

        protected override string ServiceName => "BTCPayServer.Stream.Business.Services.PriceLoaderHostedService";

        protected override double DelaySecond => 600;

        #endregion

        #region Constructors

        public PriceLoaderHostedService(
            IPriceStore priceStore,
            ICoinGeckoHttpClient coinGeckoHttpClient,
            ILogger<PriceLoaderHostedService> logger) : base(logger)
        {
            this.priceStore = priceStore;

            this.coinGeckoHttpClient = coinGeckoHttpClient;

            this.logger = logger;
        }

        #endregion

        #region Protected methods

        protected override async Task ExecutePeriodicalTaskAsync(CancellationToken cancellationToken)
        {
            try
            {
                GetExchangeRatesResponse currentPrice = await coinGeckoHttpClient.GetExchangeRatesAsync();
                foreach (string currency in currentPrice.Rates.Keys)
                {
                    priceStore.SetPrice(currency, currentPrice.Rates[currency].Value);
                }

                logger.LogDebug($"PriceLoader: Price loaded (in USD)");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot load current price of Bitcoin");
            }
        }

        #endregion
    }
}
