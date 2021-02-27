using BTCPayServer.Stream.HttpClients.CoinGeckoClient.Models.Responses;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.Abstractions
{
    public interface ICoinGeckoHttpClient : IHttpClient
    {
        #region Public methods

        Task<GetExchangeRatesResponse> GetExchangeRatesAsync();

        #endregion
    }
}
