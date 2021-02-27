using Newtonsoft.Json;
using System.Collections.Generic;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Responses
{
    public class GetInvoicePaymentMethodsResponse
    {
        #region Properties

        [JsonProperty("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("paymentLink")]
        public string PaymentLink { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }

        [JsonProperty("paymentMethodPaid")]
        public string PaymentMethodPaid { get; set; }

        [JsonProperty("totalPaid")]
        public string TotalPaid { get; set; }

        [JsonProperty("due")]
        public string Due { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("networkFee")]
        public string NetworkFee { get; set; }

        [JsonProperty("payments")]
        public IEnumerable<PaymentResponse> Payments { get; set; }

        #endregion

        public class PaymentResponse
        {
            #region Properties

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("receivedDate")]
            public string ReceivedDate { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("fee")]
            public string Fee { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("destination")]
            public string Destination { get; set; }

            #endregion
        }
    }
}
