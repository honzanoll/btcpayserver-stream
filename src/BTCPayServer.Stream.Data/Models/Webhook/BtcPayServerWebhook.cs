﻿using BTCPayServer.Stream.Data.Models.Users;
using honzanoll.Data.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BTCPayServer.Stream.Data.Models.Webhook
{
    [Table("BtcPayServer", Schema = "webhook")]
    public class BtcPayServerWebhook : ModelBase
    {
        #region Properties

        public string ExternalId { get; set; }

        public string StoreId { get; set; }

        public string Secret { get; set; }

        [NotNull]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion
    }
}
