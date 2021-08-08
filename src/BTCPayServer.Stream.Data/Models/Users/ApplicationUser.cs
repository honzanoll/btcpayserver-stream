using BTCPayServer.Stream.Common.Extensions;
using BTCPayServer.Stream.Data.Enums;
using BTCPayServer.Stream.Data.Models.OAuth;
using Microsoft.AspNetCore.Identity;
using honzanoll.Storage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BTCPayServer.Stream.Data.Models.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        #region Properties

        public string Firstame { get; set; }

        public string Surname { get; set; }

        public string BtcPayServerUri { get; set; }

        public string BtcPayServerStoreId { get; set; }

        [NotNull]
        public string DonatePageIdentifier { get; set; }

        public string StylesheetFile { get; set; }
        [NotMapped]
        public File StylesheetFileObject
        {
            get => StylesheetFile.FromJson<File>();
            set => StylesheetFile = value.ToJson();
        }

        public InvoiceCurrency? DefaultCurrency { get; set; }

        public Culture? DefaultCulture { get; set; }

        public string LogoFile { get; set; }
        [NotMapped]
        public File LogoFileObject
        {
            get => LogoFile.FromJson<File>();
            set => LogoFile = value.ToJson();
        }

        public string PageTitle { get; set; }

        public string GtagId { get; set; }

        public string MinTips { get; set; }
        [NotMapped]
        public List<MinTip> MinTipsObject
        {
            get => MinTips.FromJson<List<MinTip>>();
            set => MinTips = value.ToJson();
        }

        public virtual ICollection<StreamlabsAuthToken> StreamlabsAuthTokens { get; }

        public virtual ICollection<BtcPayServerAuthToken> BtcPayServerAuthTokens { get; }

        #endregion
    }
}
