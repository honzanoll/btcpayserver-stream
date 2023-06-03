using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Requests;
using BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Responses;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.StreamlabsClient
{
    public class StreamlabsHttpClient : BaseHttpClient<StreamlabsHttpClient>, IStreamlabsHttpClient
    {
        #region Fields

        private HttpClient httpClient;

        #endregion

        #region Constructors

        public StreamlabsHttpClient(
            HttpClient httpClient,
            ILogger<StreamlabsHttpClient> logger) : base(logger)
        {
            this.httpClient = httpClient;
        }

        #endregion

        #region Public methods

        public async Task<GetAccessTokenResponse> GetAccessTokenAsync(GetAccessTokenRequest requestData)
        {
            HttpResponseMessage response = await httpClient.PostAsync("v2.0/token", GetRequest(requestData));
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);

            return await GetResponse<GetAccessTokenResponse>(response.Content);
        }

        public async Task<SendDonateResponse> SendDonateAsync(SendDonateRequest requestData, string accessToken)
        {
            // Add access token the request header
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            HttpResponseMessage response = await httpClient.PostAsync("v2.0/donations", GetRequest(requestData));
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);

            return await GetResponse<SendDonateResponse>(response.Content);
        }

        #endregion
    }
}
