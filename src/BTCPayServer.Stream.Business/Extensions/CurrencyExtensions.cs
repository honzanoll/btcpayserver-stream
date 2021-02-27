using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Data.Enums;

namespace BTCPayServer.Stream.Business.Extensions
{
    public static class CurrencyExtensions
    {
        #region Public methods

        public static Currency ToISO(this InvoiceCurrency invoiceCurrency)
        {
            switch (invoiceCurrency)
            {
                case InvoiceCurrency.CZK:
                    return Currency.CZK;
                case InvoiceCurrency.EUR:
                    return Currency.EUR;
                case InvoiceCurrency.USD:
                default:
                    return Currency.USD;
            }
        }

        #endregion
    }
}
