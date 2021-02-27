using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Requests
{
    public class CreateInvoiceRequest
    {
        #region Properties

        [JsonProperty("amount", Required = Required.Always)]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("checkout")]
        public CheckoutRequest Checkout { get; set; }

        #endregion

        public class CheckoutRequest
        {
            #region Properties

            [JsonProperty("redirectURL")]
            public string RedirectUrl { get; set; }

            #endregion
        }
    }
}
