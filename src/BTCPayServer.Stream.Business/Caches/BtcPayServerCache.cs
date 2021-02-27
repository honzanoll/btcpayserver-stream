using BTCPayServer.Stream.Business.Caches.Abstractions;
using BTCPayServer.Stream.Business.Models.BtcPayServer;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace BTCPayServer.Stream.Business.Caches
{
    public class BtcPayServerCache : BaseCache, ICache<OAuthAuthorizedData>
    {
        #region Fields

        private readonly IMemoryCache memoryCache;

        #endregion

        #region Protected members

        protected override int ItemExpirationTime => 30;

        #endregion

        #region Private members

        private string CacheKeyPrefix
        {
            get { return $"{nameof(BtcPayServerCache)}_"; }
        }

        #endregion

        #region Constructors

        public BtcPayServerCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        #endregion

        #region Public methods

        public Guid Add(OAuthAuthorizedData data)
        {
            Guid key = Guid.NewGuid();

            memoryCache.Set(CacheKeyPrefix + key, data, CacheItemOptions);

            return key;
        }

        public bool TryGet(Guid key, out OAuthAuthorizedData data)
        {
            bool isInCache = memoryCache.TryGetValue(CacheKeyPrefix + key, out object cacheValue);
            if (isInCache)
                memoryCache.Remove(CacheKeyPrefix + key);

            data = (OAuthAuthorizedData)cacheValue;
            return isInCache;
        }

        #endregion
    }
}
