using Newtonsoft.Json;

namespace BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Responses
{
    public class SendDonateResponse
    {
        #region Properties

        [JsonProperty("donation_id")]
        public long DonationId { get; set; }

        #endregion
    }
}
