using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Responses
{
    public class CreateWebhookResponse
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        #endregion
    }
}
