using Newtonsoft.Json;
using System.Collections.Generic;

namespace BTCPayServer.Stream.HttpClients.CoinGeckoClient.Models.Responses
{
    public class GetExchangeRatesResponse
    {
        #region Properties

        [JsonProperty("rates")]
        public Dictionary<string, ExchangeRateResponse> Rates { get; set; }

        #endregion

        public class ExchangeRateResponse
        {
            #region Properties

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("unit")]
            public string Unit { get; set; }

            [JsonProperty("value")]
            public double Value { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            #endregion
        }
    }
}
