using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;

namespace BTCPayServer.Stream.Business.Caches
{
    public abstract class BaseCache
    {
        #region Properties

        protected abstract int ItemExpirationTime { get; }

        protected virtual MemoryCacheEntryOptions CacheItemOptions
        {
            get
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(ItemExpirationTime));

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AddExpirationToken(new CancellationChangeToken(cancellationTokenSource.Token));
                return options;
            }
        }

        #endregion
    }
}
