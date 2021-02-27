using BTCPayServer.Stream.Business.Caches;
using BTCPayServer.Stream.Business.Caches.Abstractions;
using BTCPayServer.Stream.Business.Converters;
using BTCPayServer.Stream.Business.Converters.Abstractions;
using BTCPayServer.Stream.Business.Models.BtcPayServer;
using BTCPayServer.Stream.Business.Services;
using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.HttpClients.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BTCPayServer.Stream.Business.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Public methods

        public static void AddStreamlabsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConverters();

            services.AddStreamlabsHttpClient(configuration);
            services.AddScoped<IStreamlabsService, StreamlabsService>();
        }

        public static void AddBtcPayServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBtcPayServerHttpClient(configuration);
            services.AddScoped<IBtcPayServerService, BtcPayServerService>();
        }

        public static void AddPriceStoreService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoindeskHttpClient(configuration);
            services.AddCoinGeckoHttpClient(configuration);
            services.AddHostedService<PriceLoaderHostedService>();
            services.AddSingleton<IPriceStore, PriceStore>();
        }

        public static void AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddScoped<ICache<OAuthAuthorizedData>, BtcPayServerCache>();
        }

        public static void AddConverters(this IServiceCollection services)
        {
            services.AddScoped<IConverter<EmojiConverter>, EmojiConverter>();
        }

        #endregion
    }
}
