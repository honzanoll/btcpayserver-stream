using BTCPayServer.Stream.Data.Enums;
using BTCPayServer.Stream.Data.Models.Users;
using System;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Business.Services.Abstractions
{
    public interface IBtcPayServerService
    {
        #region Public methods

        Task InitConnectionAsync(string apiKey, Guid userId);

        Task<Uri> CreateInvoiceAsync(decimal amount, string donator, string message, InvoiceCurrency invoiceCurrency, ApplicationUser user);

        Task ProcessReceivedPayment(string invoiceId);

        Task ClearInvoice(string invoiceId);

        Task ClearConnectionAsync(Guid userId);

        #endregion
    }
}
