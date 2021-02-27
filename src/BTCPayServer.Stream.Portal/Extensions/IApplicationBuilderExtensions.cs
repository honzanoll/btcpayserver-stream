using BTCPayServer.Stream.Portal.Middlewares;
using BTCPayServer.Stream.Portal.Middlewares.Consts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace BTCPayServer.Stream.Portal.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void MapFileMiddlewares(this IApplicationBuilder app)
        {
            app.MapWhen(context => context.Request.Path.ToString().EndsWith(Suffixes.Stylesheet),
                    appBranch => appBranch.UseMiddleware<StylesheetMiddleware>());

            app.MapWhen(context => context.Request.Path.ToString().EndsWith(Suffixes.Logo),
                    appBranch => appBranch.UseMiddleware<LogoMiddleware>());
        }

        public static void UseLocalization(this IApplicationBuilder app)
        {
            CultureInfo csCultureInfo = new CultureInfo("cs-CZ");
            csCultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            List<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                csCultureInfo
            };

            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>()
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                }
            });
        }
    }
}
