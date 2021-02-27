using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.CoindeskClient.Models.Responses;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.CoindeskClient
{
    public class CoindeskHttpClient : BaseHttpClient<CoindeskHttpClient>, ICoindeskHttpClient
    {
        #region Fields

        private readonly HttpClient httpClient;

        #endregion

        #region Constructors

        public CoindeskHttpClient(
            HttpClient httpClient,
            ILogger<CoindeskHttpClient> logger) : base(logger)
        {
            this.httpClient = httpClient;
        }

        #endregion

        #region Public methods

        public async Task<GetCurrentPriceResponse> GetCurrentPriceAsync(string currency)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"v1/bpi/currentprice/{currency}.json");

            await EnsureValidResponseAsync(response);

            return await GetResponse<GetCurrentPriceResponse>(response.Content);
        }

        #endregion
    }
}
