using BTCPayServer.Stream.Data.Models.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BTCPayServer.Stream.Data.Models.OAuth
{
    [Table("BtcPayServer", Schema = "oAuth")]
    public class BtcPayServerAuthToken : AuthToken
    {

        #region Properties

        [NotNull]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion
    }
}
