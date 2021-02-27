using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Responses
{
    public class CreateInvoiceResponse
    {
        #region Properties

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("checkoutLink")]
        public string CheckoutLink { get; set; }

        #endregion
    }
}
