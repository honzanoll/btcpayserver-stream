using BTCPayServer.Stream.Common.Models.Settings.HttpClients;
using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.BtcPayServerClient;
using BTCPayServer.Stream.HttpClients.CoindeskClient;
using BTCPayServer.Stream.HttpClients.CoinGeckoClient;
using BTCPayServer.Stream.HttpClients.Factories;
using BTCPayServer.Stream.HttpClients.Factories.Abstractions;
using BTCPayServer.Stream.HttpClients.StreamlabsClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace BTCPayServer.Stream.HttpClients.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Public methods

        public static void AddStreamlabsHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            StreamlabsSettings streamlabsSettings = configuration.GetSection(nameof(StreamlabsSettings)).Get<StreamlabsSettings>();

            services.AddHttpClient<IStreamlabsHttpClient, StreamlabsHttpClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(streamlabsSettings.BaseApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }

        public static void AddBtcPayServerHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            BtcPayServerSettings btcPayServerSettings = configuration.GetSection(nameof(BtcPayServerSettings)).Get<BtcPayServerSettings>();

            services.AddScoped<IHttpClientFactory<BtcPayServerHttpClient>, BtcPayServerHttpClientFactory>();
            services.AddScoped<IBtcPayServerHttpClient, BtcPayServerHttpClient>();
        }

        public static void AddCoindeskHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            CoindeskSettings coindeskSettings = configuration.GetSection(nameof(CoindeskSettings)).Get<CoindeskSettings>();

            services.AddHttpClient<ICoindeskHttpClient, CoindeskHttpClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(coindeskSettings.BaseApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }

        public static void AddCoinGeckoHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            CoinGeckoSettings coinGeckoSettings = configuration.GetSection(nameof(CoinGeckoSettings)).Get<CoinGeckoSettings>();

            services.AddHttpClient<ICoinGeckoHttpClient, CoinGeckoHttpClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(coinGeckoSettings.BaseApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }

        #endregion
    }
}
