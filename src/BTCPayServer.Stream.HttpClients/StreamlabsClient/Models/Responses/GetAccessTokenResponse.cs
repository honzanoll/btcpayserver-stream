using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Responses
{
    public class GetAccessTokenResponse
    {
        #region Properties

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        #endregion
    }
}
