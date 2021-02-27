using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Business.Services.Abstractions;
using System.Collections.Concurrent;

namespace BTCPayServer.Stream.Business.Services
{
    public class PriceStore : IPriceStore
    {
        #region Fields

        private readonly ConcurrentDictionary<string, double> prices;

        #endregion

        #region Constructors

        public PriceStore()
        {
            prices = new ConcurrentDictionary<string, double>();
        }

        #endregion

        #region Public methods

        public void SetPrice(Currency currency, double price)
        {
            prices.AddOrUpdate(currency.ToString(), price, (string currency, double currentPrice) => price);
        }

        public void SetPrice(string currency, double price)
        {
            prices.AddOrUpdate(currency.ToUpper(), price, (string currency, double currentPrice) => price);
        }

        public double GetPrice(Currency currency)
        {
            if (prices.TryGetValue(currency.ToString(), out double price))
                return price;

            return 1;
        }

        #endregion
    }
}
