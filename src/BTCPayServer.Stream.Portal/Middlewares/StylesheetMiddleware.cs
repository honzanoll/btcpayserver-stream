using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Portal.Middlewares.Consts;
using BTCPayServer.Stream.Portal.Middlewares.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using honzanoll.Storage.NetCore.Abstractions;
using honzanoll.Storage.Models.Settings;
using honzanoll.Web.Middlewares;
using honzanoll.Web.Middlewares.Extensions;
using System;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Portal.Middlewares
{
    public class StylesheetMiddleware : MiddlewareBase
    {
        #region Fields

        private readonly IStorageProvider<LocalStorage, StorageType> localStorageProvider;

        private readonly IServiceProvider serviceProvider;

        private readonly ILogger<StylesheetMiddleware> logger;

        #endregion

        #region Constructors

        public StylesheetMiddleware(
            IStorageProvider<LocalStorage, StorageType> localStorageProvider,
            IServiceProvider serviceProvider,
            ILogger<StylesheetMiddleware> logger)
        {
            this.localStorageProvider = localStorageProvider;

            this.serviceProvider = serviceProvider;

            this.logger = logger;
        }

        #endregion

        #region Public methods

        public override async Task InvokeAsync(
            HttpContext httpContext,
            RequestDelegate next)
        {
            httpContext.Response.Clear();

            try
            {
                string identifier = httpContext.EnsureValidParameter(Parameters.Identifier);

                await MakeFileResponseAsync(httpContext, await localStorageProvider.GetStylesheetAsync(identifier, serviceProvider), "text/css", "stylesheet");

            }
            catch (Exception e)
            {
                await HandleErrorAsync(e, logger, httpContext);
            }
        }

        #endregion
    }
}
