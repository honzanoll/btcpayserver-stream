using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Business.Extensions;
using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.Common.Models.Settings;
using BTCPayServer.Stream.Data.Enums;
using BTCPayServer.Stream.Data.Models.Invoices;
using BTCPayServer.Stream.Data.Models.OAuth;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Data.Models.Webhook;
using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models;
using BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Requests;
using BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models.Responses;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using honzanoll.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Business.Services
{
    public class BtcPayServerService : IBtcPayServerService
    {
        #region Fields

        private readonly IStreamlabsService streamlabsService;

        private readonly IBtcPayServerHttpClient btcPayServerHttpClient;

        private readonly IRepository<BtcPayServerAuthToken> btcPayServerAuthTokenRepository;
        private readonly IRepository<BtcPayServerWebhook> btcPayServerWebhookRepository;
        private readonly IRepository<Invoice> invoiceRepository;
        private readonly IUserRepository userRepository;

        private readonly GlobalSettings globalSettings;

        private ILogger<BtcPayServerService> logger;

        #endregion

        #region Constructors

        public BtcPayServerService(
            IStreamlabsService streamlabsService,
            IBtcPayServerHttpClient btcPayServerHttpClient,
            IRepository<BtcPayServerAuthToken> btcPayServerAuthTokenRepository,
            IRepository<BtcPayServerWebhook> btcPayServerWebhookRepository,
            IRepository<Invoice> invoiceRepository,
            IUserRepository userRepository,
            IOptions<GlobalSettings> globalSettingsOptions,
            ILogger<BtcPayServerService> logger)
        {
            this.streamlabsService = streamlabsService;

            this.btcPayServerHttpClient = btcPayServerHttpClient;

            this.btcPayServerAuthTokenRepository = btcPayServerAuthTokenRepository;
            this.btcPayServerWebhookRepository = btcPayServerWebhookRepository;
            this.invoiceRepository = invoiceRepository;
            this.userRepository = userRepository;

            globalSettings = globalSettingsOptions.Value;

            this.logger = logger;
        }

        #endregion

        #region Public methods

        public async Task InitConnectionAsync(string apiKey, Guid userId)
        {
            await btcPayServerAuthTokenRepository.CreateAsync(new BtcPayServerAuthToken
            {
                Token = apiKey,
                UserId = userId,
                Created = DateTime.Now,
                CreatedBy = "SYSTEM"
            });

            (string baseAddress, string storeId) = await userRepository.GetBtcPayServerDataAsync(userId);

            CreateWebhookResponse response = await btcPayServerHttpClient.CreateWebhookAsync(baseAddress, apiKey, storeId, new CreateWebhookRequest
            {
                Id = Guid.NewGuid().ToString(),
                Url = globalSettings.Infrastructure.FEUrl + "/webhooks/btcpayserver/invoice-event",
                AuthorizedEvents = new CreateWebhookRequest.AuthorizedEventsResponse
                {
                    Everything = false,
                    SpecificEvents = new WebhookSpecificEvent[] {
                            WebhookSpecificEvent.InvoiceReceivedPayment,
                            WebhookSpecificEvent.InvoiceExpired
                        }
                },
                Secret = Guid.NewGuid().ToString()
            });

            await btcPayServerWebhookRepository.CreateAsync(new BtcPayServerWebhook
            {
                ExternalId = response.Id,
                StoreId = storeId,
                Secret = response.Secret,
                UserId = userId,
                Created = DateTime.Now,
                CreatedBy = "SYSTEM"
            });
        }

        public async Task<Uri> CreateInvoiceAsync(decimal amount, string donator, string message, InvoiceCurrency currency, ApplicationUser user)
        {
            string baseAddress = user.BtcPayServerUri;
            string accessToken = await GetAccessTokenAsync(user.Id);

            string requestAmount;
            string requestCurrency;
            if (currency == InvoiceCurrency.SAT)
            {
                requestAmount = (amount / 100000000).ToString();
                requestCurrency = Currency.BTC.ToString();
            }
            else
            {
                requestAmount = amount.ToString();
                requestCurrency = currency.ToISO().ToString();
            }

            CreateInvoiceResponse response = await btcPayServerHttpClient.CreateInvoiceAsync(baseAddress, accessToken, user.BtcPayServerStoreId, new CreateInvoiceRequest
            {
                Amount = requestAmount,
                Currency = requestCurrency,
                Checkout = new CreateInvoiceRequest.CheckoutRequest
                {
                    RedirectUrl = globalSettings.Infrastructure.FEUrl + "/donate/acknowledgment/{InvoiceId}"
                }
            });

            await invoiceRepository.CreateAsync(new Invoice
            {
                ExternalId = response.Id,
                StoreId = user.BtcPayServerStoreId,
                Amount = amount,
                Currency = currency,
                Donator = donator,
                Message = message,
                UserId = user.Id,
                Created = DateTime.Now,
                CreatedBy = "SYSTEM"
            });

            if (!string.IsNullOrEmpty(response.CheckoutLink))
                return new Uri(response.CheckoutLink);

            return new Uri(new Uri(baseAddress), $"i/{response.Id}");
        }

        public async Task ProcessReceivedPayment(string invoiceId)
        {
            Invoice invoice = await invoiceRepository.GetAsync(i => i.ExternalId == invoiceId);
            if (invoice == null || invoice.Processed)
                return;

            IEnumerable<GetInvoicePaymentMethodsResponse> invoicePaymentInfo = null;
            try
            {
                invoicePaymentInfo = await GetInvoicePaymentInfoAsync(invoice);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, $"Cannot get invoice (ExternalId: {invoice.ExternalId}) payment methods");
            }

            string donator = string.IsNullOrEmpty(invoice.Donator) ? "Anonymous" : invoice.Donator;

            string paymentType = string.Empty;
            string donateValue = string.Empty;
            GetInvoicePaymentMethodsResponse usedPaymentMethod = invoicePaymentInfo?.First(ipi => ipi.PaymentMethodPaid != "0");
            if (usedPaymentMethod != null)
            {
                if (usedPaymentMethod.PaymentMethod == "BTC-LightningNetwork")
                    paymentType = "[LN] ";
                else
                    paymentType = "[BTC] ";

                donateValue = $" {decimal.Parse(usedPaymentMethod.TotalPaid) * 100000000:0} SAT";
            }

            await streamlabsService.SendDonateAsync(invoice.UserId, new Models.Streamlabs.Donate
            {
                Name = donator,
                Message = paymentType + invoice.Message + donateValue,
                Identifier = donator,
                Amount = (double)invoice.Amount,
                Currency = invoice.Currency
            });

            // Mark invoice as processed
            invoice.Processed = true;
            invoice.Updated = DateTime.Now;
            invoice.UpdatedBy = "SYSTEM";
            await invoiceRepository.UpdateAsync(invoice);
        }

        public async Task ClearInvoice(string invoiceId)
        {
            Invoice invoice = await invoiceRepository.GetAsync(i => i.ExternalId == invoiceId);
            if (invoice == null)
                return;

            await invoiceRepository.DeleteAsync(invoice);
        }

        public async Task ClearConnectionAsync(Guid userId)
        {
            BtcPayServerAuthToken btcPayServerAuthToken = await btcPayServerAuthTokenRepository.GetAsync(bpsat => bpsat.UserId == userId);
            if (btcPayServerAuthToken == null)
                return;

            (string baseAddress, string storeId) = await userRepository.GetBtcPayServerDataAsync(userId);

            // Clear registered webhooks
            List<BtcPayServerWebhook> webhooks = await btcPayServerWebhookRepository.GetAllAsync(bpsw => bpsw.UserId == userId);
            foreach (BtcPayServerWebhook webhook in webhooks)
            {
                await btcPayServerHttpClient.DeleteWebhookAsync(baseAddress, btcPayServerAuthToken.Token, webhook.StoreId, webhook.ExternalId);

                await btcPayServerWebhookRepository.DeleteAsync(webhook);
            }

            // Revoke current api key
            await btcPayServerHttpClient.RevokeTokenAsync(baseAddress, btcPayServerAuthToken.Token);

            // Remove the token from local database
            await btcPayServerAuthTokenRepository.DeleteAsync(btcPayServerAuthToken);
        }

        #endregion

        #region Private methods

        public async Task<string> GetAccessTokenAsync(Guid userId)
        {
            BtcPayServerAuthToken btcPayServerAuthToken = await btcPayServerAuthTokenRepository.GetAsync(bpsat => bpsat.UserId == userId);
            if (btcPayServerAuthToken == null)
                return null;

            return btcPayServerAuthToken.Token;
        }

        private async Task<IEnumerable<GetInvoicePaymentMethodsResponse>> GetInvoicePaymentInfoAsync(Invoice invoice)
        {
            ApplicationUser applicationUser = await userRepository.GetAsync(invoice.UserId);

            string baseAddress = applicationUser.BtcPayServerUri;
            string accessToken = await GetAccessTokenAsync(invoice.UserId);

            return await btcPayServerHttpClient.GetInvoicePaymentMethodAsync(baseAddress, accessToken, new GetInvoicePaymentMethodsRequest
            {
                StoreId = invoice.StoreId,
                InvoiceId = invoice.ExternalId
            });
        }

        #endregion
    }
}
