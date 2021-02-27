using BTCPayServer.Stream.Data.Enums;
using BTCPayServer.Stream.Data.Models.Users;
using honzanoll.Data.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BTCPayServer.Stream.Data.Models.Invoices
{
    public class Invoice : ModelBase
    {
        #region Properties

        public string ExternalId { get; set; }

        public string StoreId { get; set; }

        public decimal Amount { get; set; }

        public InvoiceCurrency Currency { get; set; }

        public string Donator { get; set; }

        public string Message { get; set; }

        public bool Processed { get; set; }

        [NotNull]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion
    }
}
