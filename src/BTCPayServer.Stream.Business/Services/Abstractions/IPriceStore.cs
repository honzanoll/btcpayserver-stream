using BTCPayServer.Stream.Business.Consts.Enums;

namespace BTCPayServer.Stream.Business.Services.Abstractions
{
    public interface IPriceStore
    {
        #region Public methods

        void SetPrice(Currency currency, double price);

        void SetPrice(string currency, double price);

        double GetPrice(Currency currency);

        #endregion
    }
}
