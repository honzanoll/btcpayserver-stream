using BTCPayServer.Stream.Business.Models.BtcPayServer;
using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.Common.Extensions;
using BTCPayServer.Stream.Data.Models.Webhook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using honzanoll.Repository.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Portal.Controllers.Webhooks
{
    [Route("webhooks/btcpayserver")]
    [AllowAnonymous]
    public class BtcPayServerWebhookController : Controller
    {
        #region Fields

        private readonly IBtcPayServerService btcPayServerService;

        private readonly IRepository<BtcPayServerWebhook> btcPayServerWebhookRepository;

        private readonly ILogger<BtcPayServerWebhookController> logger;

        #endregion

        #region Constructors

        public BtcPayServerWebhookController(
            IBtcPayServerService btcPayServerService,
            IRepository<BtcPayServerWebhook> btcPayServerWebhookRepository,
            ILogger<BtcPayServerWebhookController> logger)
        {
            this.btcPayServerService = btcPayServerService;

            this.btcPayServerWebhookRepository = btcPayServerWebhookRepository;

            this.logger = logger;
        }

        #endregion

        [HttpPost("invoice-event")]
        public async Task<IActionResult> InvoiceEvent()
        {
            try
            {
                string payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                InvoiceWebhook data = payload.FromJson<InvoiceWebhook>();

                logger.LogInformation("Received invoice event: " + data.Type + " (data : " + data.ToJson() + ")");

                if (!HttpContext.Request.Headers.TryGetValue("BTCPay-Sig", out StringValues sig))
                    return Unauthorized();

                string signature = sig.FirstOrDefault().Replace("sha256=", "");

                // Check if is webhook valid (by signature comparsion)
                IActionResult validationResult = await ValidateSignature(signature, data.WebhookId, payload);
                if (validationResult != null)
                    return validationResult;

                switch (data.Type)
                {
                    case "InvoiceReceivedPayment":
                        await btcPayServerService.ProcessReceivedPayment(data.InvoiceId);
                        break;
                    case "InvoiceExpired":
                        await btcPayServerService.ClearInvoice(data.InvoiceId);
                        break;
                    default:
                        logger.LogWarning($"Unknown webhook received (type: {data.Type})");
                        break;
                }

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Something went wrong during webhooks processing");

                return StatusCode(500);
            }
        }

        #region Private methods

        private async Task<IActionResult> ValidateSignature(string signature, string webhookId, string payload)
        {
            BtcPayServerWebhook webhook = await btcPayServerWebhookRepository.GetAsync(btsw => btsw.ExternalId == webhookId);
            if (webhook == null)
                return Unauthorized();

            byte[] secret = Encoding.UTF8.GetBytes(webhook.Secret);

            using (HMACSHA256 hmac = new HMACSHA256(secret))
            {
                string hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)).ToHexString();

                if (!hash.Equals(signature))
                    return Forbid();
            }

            return null;
        }

        #endregion
    }
}
