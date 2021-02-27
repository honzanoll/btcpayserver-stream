using BTCPayServer.Stream.Portal.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace BTCPayServer.Stream.Portal.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Public methods

        public static void AddFileMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<StylesheetMiddleware>();
            services.AddScoped<LogoMiddleware>();
        }

        #endregion
    }
}
