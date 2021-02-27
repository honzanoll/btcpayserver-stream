namespace BTCPayServer.Stream.Common.Models.Settings
{
    public class GlobalSettings
    {
        #region Properties

        public InfrastructureSettings Infrastructure { get; set; }

        #endregion

        public class InfrastructureSettings
        {
            #region Properties

            public string FEUrl { get; set; }

            #endregion
        }
    }
}
