using Newtonsoft.Json;

namespace BTCPayServer.Stream.Business.Models.BtcPayServer
{
    public class InvoiceWebhook
    {
        #region Properties

        [JsonProperty("deliveryId")]
        public string DeliveryId { get; set; }

        [JsonProperty("webhookId")]
        public string WebhookId { get; set; }

        [JsonProperty("originalDeliveryId")]
        public string OriginalDeliveryId { get; set; }

        [JsonProperty("isRedelivery")]
        public bool IsRedelivery { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("partiallyPaid")]
        public bool PartiallyPaid { get; set; }

        [JsonProperty("afterExpiration")]
        public bool AfterExpiration { get; set; }

        #endregion
    }
}
