using Newtonsoft.Json;
using System.Collections.Generic;

namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Requests
{
    public class CreateWebhookRequest
    {
        #region Properties

        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("enabled", Required = Required.Always)]
        public bool Enabled { get; set; } = true;

        [JsonProperty("automaticRedelivery", Required = Required.Always)]
        public bool AutomaticRedelivery { get; set; } = true;

        [JsonProperty("url", Required = Required.Always)]
        public string Url { get; set; }

        [JsonProperty("authorizedEvents", Required = Required.Always)]
        public AuthorizedEventsResponse AuthorizedEvents { get; set; }

        [JsonProperty("secret", Required = Required.Always)]
        public string Secret { get; set; }

        #endregion

        public class AuthorizedEventsResponse
        {
            #region Properties

            [JsonProperty("everything", Required = Required.Always)]
            public bool Everything { get; set; } = true;

            [JsonProperty("specificEvents")]
            public IEnumerable<WebhookSpecificEvent> SpecificEvents { get; set; }

            #endregion
        }
    }
}
