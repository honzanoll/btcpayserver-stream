using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Requests
{
    public class GetAccessTokenRequest
    {
        #region Properties

        [JsonProperty("grant_type", Required = Required.Always)]
        public string GrantType { get; set; } = "authorization_code";

        [JsonProperty("client_id", Required = Required.Always)]
        public string ClientId { get; set; }

        [JsonProperty("client_secret", Required = Required.Always)]
        public string ClientSecret { get; set; }

        [JsonProperty("redirect_uri", Required = Required.Always)]
        public string RedirectUri { get; set; }

        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; }

        #endregion
    }
}
