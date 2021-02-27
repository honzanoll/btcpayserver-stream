using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Requests;
using BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Responses;
using BTCPayServer.Stream.HttpClients.Factories.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient
{
    public class BtcPayServerHttpClient : BaseHttpClient<BtcPayServerHttpClient>, IBtcPayServerHttpClient
    {
        #region Fields

        private readonly IHttpClientFactory<BtcPayServerHttpClient> httpClientFactory;

        #endregion

        #region Constructors

        public BtcPayServerHttpClient(
            IHttpClientFactory<BtcPayServerHttpClient> httpClientFactory,
            ILogger<BtcPayServerHttpClient> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Public methods

        public async Task RevokeTokenAsync(string baseAddress, string accessToken)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(baseAddress, accessToken);

            HttpResponseMessage response = await httpClient.DeleteAsync("api/v1/api-keys/current");
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);
        }

        public async Task<IEnumerable<GetStoreResponse>> GetStoresAsync(string baseAddress, string accessToken)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(baseAddress, accessToken);

            HttpResponseMessage response = await httpClient.GetAsync("api/v1/stores");
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);

            return await GetResponse<List<GetStoreResponse>>(response.Content);
        }

        public async Task<CreateWebhookResponse> CreateWebhookAsync(string baseAddress, string accessToken, string storeId, CreateWebhookRequest requestData)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(baseAddress, accessToken);

            HttpResponseMessage response = await httpClient.PostAsync($"api/v1/stores/{storeId}/webhooks", GetRequest(requestData));
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);

            return await GetResponse<CreateWebhookResponse>(response.Content);
        }

        public async Task DeleteWebhookAsync(string baseAddress, string accessToken, string storeId, string webhookId)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(baseAddress, accessToken);

            HttpResponseMessage response = await httpClient.DeleteAsync($"api/v1/stores/{storeId}/webhooks/{webhookId}");
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);
        }

        public async Task<CreateInvoiceResponse> CreateInvoiceAsync(string baseAddress, string accessToken, string storeId, CreateInvoiceRequest requestData)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(baseAddress, accessToken);

            HttpResponseMessage response = await httpClient.PostAsync($"api/v1/stores/{storeId}/invoices", GetRequest(requestData));
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);

            return await GetResponse<CreateInvoiceResponse>(response.Content);
        }

        public async Task<GetInvoiceResponse> GetInvoiceAsync(string baseAddress, string accessToken, GetInvoiceRequest requestData)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(baseAddress, accessToken);

            HttpResponseMessage response = await httpClient.GetAsync($"api/v1/stores/{requestData.StoreId}/invoices/{requestData.InvoiceId}");
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);

            return await GetResponse<GetInvoiceResponse>(response.Content);
        }

        public async Task<IEnumerable<GetInvoicePaymentMethodsResponse>> GetInvoicePaymentMethodAsync(string baseAddress, string accessToken, GetInvoicePaymentMethodsRequest requestData)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(baseAddress, accessToken);

            HttpResponseMessage response = await httpClient.GetAsync($"api/v1/stores/{requestData.StoreId}/invoices/{requestData.InvoiceId}/payment-methods");
            httpClient.Dispose();

            await EnsureValidResponseAsync(response);

            return await GetResponse<List<GetInvoicePaymentMethodsResponse>>(response.Content);
        }

        #endregion
    }
}
