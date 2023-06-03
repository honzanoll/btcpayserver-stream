using BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Requests;
using BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Responses;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.Abstractions
{
    public interface IStreamlabsHttpClient : IHttpClient
    {
        #region Public methods

        Task<GetAccessTokenResponse> GetAccessTokenAsync(GetAccessTokenRequest requestData);

        Task<SendDonateResponse> SendDonateAsync(SendDonateRequest requestData, string accessToken);

        #endregion
    }
}
