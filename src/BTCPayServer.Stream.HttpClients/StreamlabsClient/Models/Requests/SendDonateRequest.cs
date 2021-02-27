using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Requests
{
    public class SendDonateRequest
    {
        #region Properties

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("identifier", Required = Required.Always)]
        public string Identifier { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public double Amount { get; set; }

        [JsonProperty("currency", Required = Required.Always)]
        public string Currency { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("access_token", Required = Required.Always)]
        public string AccessToken { get; set; }

        [JsonProperty("skip_alert")]
        public string SkipAlert { get; set; }

        #endregion
    }
}
