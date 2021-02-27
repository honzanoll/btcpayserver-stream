using BTCPayServer.Stream.HttpClients.CoindeskClient.Models.Responses;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.Abstractions
{
    public interface ICoindeskHttpClient : IHttpClient
    {
        #region Public methods

        Task<GetCurrentPriceResponse> GetCurrentPriceAsync(string currency);

        #endregion
    }
}
