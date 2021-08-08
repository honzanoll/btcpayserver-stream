using BTCPayServer.Stream.Data.Enums;

namespace BTCPayServer.Stream.Data.Models.Users
{
    public class MinTip
    {
        #region Properties

        public InvoiceCurrency Currency { get; set; }

        public decimal MinValue { get; set; }

        #endregion
    }
}
