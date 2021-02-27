using BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Requests;
using BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients.Abstractions
{
    public interface IBtcPayServerHttpClient : IHttpClient
    {
        #region Public methods

        Task RevokeTokenAsync(string baseAddress, string accessToken);

        Task<IEnumerable<GetStoreResponse>> GetStoresAsync(string baseAddress, string accessToken);

        Task<CreateWebhookResponse> CreateWebhookAsync(string baseAddress, string accessToken, string storeId, CreateWebhookRequest requestData);

        Task DeleteWebhookAsync(string baseAddress, string accessToken, string storeId, string webhookId);

        Task<CreateInvoiceResponse> CreateInvoiceAsync(string baseAddress, string accessToken, string storeId, CreateInvoiceRequest requestData);

        Task<GetInvoiceResponse> GetInvoiceAsync(string baseAddress, string accessToken, GetInvoiceRequest requestData);

        Task<IEnumerable<GetInvoicePaymentMethodsResponse>> GetInvoicePaymentMethodAsync(string baseAddress, string accessToken, GetInvoicePaymentMethodsRequest requestData);

        #endregion
    }
}
