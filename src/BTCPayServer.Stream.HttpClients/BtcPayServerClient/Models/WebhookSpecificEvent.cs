namespace BTCPayServer.Stream.HttpClients.BtcPayServerClient.Models
{
    public enum WebhookSpecificEvent
    {
        InvoiceCreated,

        InvoiceReceivedPayment,

        InvoiceProcessing,

        InvoiceExpired,

        InvoiceSettled,

        InvoiceInvalid
    }
}
