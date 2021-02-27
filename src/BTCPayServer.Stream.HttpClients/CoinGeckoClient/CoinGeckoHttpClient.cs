using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.CoinGeckoClient.Models.Responses;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.CoinGeckoClient
{
    public class CoinGeckoHttpClient : BaseHttpClient<CoinGeckoHttpClient>, ICoinGeckoHttpClient
    {
        #region Fields

        private readonly HttpClient httpClient;

        #endregion

        #region Constructors

        public CoinGeckoHttpClient(
            HttpClient httpClient,
            ILogger<CoinGeckoHttpClient> logger) : base(logger)
        {
            this.httpClient = httpClient;
        }

        #endregion

        #region Public methods

        public async Task<GetExchangeRatesResponse> GetExchangeRatesAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync($"v3/exchange_rates");

            await EnsureValidResponseAsync(response);

            return await GetResponse<GetExchangeRatesResponse>(response.Content);
        }

        #endregion
    }
}
