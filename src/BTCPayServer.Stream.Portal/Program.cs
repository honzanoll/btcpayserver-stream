using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using honzanoll.Common.Extensions;
using honzanoll.Logging.Extensions;
using IConfigurationExtensions = BTCPayServer.Stream.Common.Extensions.IConfigurationExtensions;

namespace BTCPayServer.Stream.Portal
{
    public class Program
    {
        #region Public methods

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureNCAppConfiguration(IConfigurationExtensions.GetAppSettingsFileRelativePath)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureNCLogging();

        #endregion
    }
}
