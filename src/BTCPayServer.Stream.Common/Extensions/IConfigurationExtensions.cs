namespace BTCPayServer.Stream.Common.Extensions
{
    public class IConfigurationExtensions
    {
        #region Public methods

        public static string GetAppSettingsFileRelativePath()
        {
#if DEBUG
            return "appsettings.localhost.json";
#else
            return "appsettings.json";
#endif
        }

        #endregion
    }
}
