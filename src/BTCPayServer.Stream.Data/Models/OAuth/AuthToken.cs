using honzanoll.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace BTCPayServer.Stream.Data.Models.OAuth
{
    public class AuthToken : ModelBase
    {
        #region Properties

        [NotNull]
        public string Token { get; set; }

        #endregion
    }
}
