using BTCPayServer.Stream.Business.Converters.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BTCPayServer.Stream.Business.Converters
{
    public abstract class BaseConverter : ReadOnlyDictionary<string, string>, IConverter<BaseConverter>
    {
        #region Contructors

        protected BaseConverter(IDictionary<string, string> dictionary) : base(dictionary) { }

        #endregion

        #region Public methods

        public string Convert(string text)
        {
            return Keys.Aggregate(text, (current, key) => current.Replace($" {key} ", $" {this[key]} "));
        }

        #endregion
    }
}
