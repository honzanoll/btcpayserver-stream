using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Requests
{
    public class GetInvoicePaymentMethodsRequest
    {
        #region Properties
        
        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }

        #endregion
    }
}
