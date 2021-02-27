using System;
using System.Collections.Generic;

namespace BTCPayServer.Stream.Business.Models.BtcPayServer
{
    public class OAuthAuthorizedData
    {
        #region Properties

        public string ApiKey { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<string> Permissions { get; set; }

        #endregion
    }
}
