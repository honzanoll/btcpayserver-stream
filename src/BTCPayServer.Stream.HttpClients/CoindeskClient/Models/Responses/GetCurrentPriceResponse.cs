using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BTCPayServer.Stream.HttpClients.CoindeskClient.Models.Responses
{
    public class GetCurrentPriceResponse
    {
        #region Properties

        [JsonProperty("time")]
        public TimeResponse Time { get; set; }

        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonProperty("bpi")]
        public IDictionary<string, BpiResponse> Bpi { get; set; }

        #endregion

        public class TimeResponse
        {
            #region Properties

            [JsonProperty("updatedISO")]
            public DateTime Date { get; set; }

            #endregion
        }

        public class BpiResponse
        {
            #region Properties

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("rate")]
            public string Rate { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("rate_float")]
            public double RateNumber { get; set; }

            #endregion
        }
    }
}
