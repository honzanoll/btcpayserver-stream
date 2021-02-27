using BTCPayServer.Stream.Common.Models.Settings;
using BTCPayServer.Stream.Common.Models.Settings.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BTCPayServer.Stream.Common.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Public methods

        public static void AddCommonConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GlobalSettings>(configuration.GetSection(nameof(GlobalSettings)));

            services.Configure<StreamlabsSettings>(configuration.GetSection(nameof(StreamlabsSettings)));
            services.Configure<CoindeskSettings>(configuration.GetSection(nameof(CoindeskSettings)));
            services.Configure<BtcPayServerSettings>(configuration.GetSection(nameof(BtcPayServerSettings)));
        }

        #endregion
    }
}
