namespace BTCPayServer.Stream.Portal.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        #region Properties

        public bool IsBtcPayServerConnected { get; set; }

        public bool IsStreamlabsConnected { get; set; }

        /// <summary>
        /// Current price of Bitcoin (in USD)
        /// </summary>
        public double BitcoinCurrentPrice { get; set; }
        public string BitcoinCurrentPriceText => "$" + BitcoinCurrentPrice.ToString("#.00");

        public string DonationUrl { get; set; }

        #endregion
    }
}
