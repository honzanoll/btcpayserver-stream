using System;

namespace BTCPayServer.Stream.Business.Caches.Abstractions
{
    public interface ICache<TModel> where TModel : new()
    {
        #region Public methods

        Guid Add(TModel data);

        bool TryGet(Guid key, out TModel data);

        #endregion
    }
}
