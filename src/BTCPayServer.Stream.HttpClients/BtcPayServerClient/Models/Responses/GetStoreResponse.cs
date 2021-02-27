using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Responses
{
    public class GetStoreResponse
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        #endregion
    }
}
