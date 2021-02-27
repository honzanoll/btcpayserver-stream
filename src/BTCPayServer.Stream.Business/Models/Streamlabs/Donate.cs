using BTCPayServer.Stream.Data.Enums;

namespace BTCPayServer.Stream.Business.Models.Streamlabs
{
    public class Donate
    {
        #region Properties

        public string Name { get; set; }

        public string Message { get; set; }

        public string Identifier { get; set; }

        public double Amount { get; set; }

        public InvoiceCurrency Currency { get; set; }

        #endregion
    }
}
