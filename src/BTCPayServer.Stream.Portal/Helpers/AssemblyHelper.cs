using System.Reflection;

namespace BTCPayServer.Stream.Portal.Helpers
{
    public static class AssemblyHelper
    {
        #region Public methods

        public static string GetAssemblyInformationalVersion()
        {
            return Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion ?? string.Empty;
        }

        #endregion
    }
}
