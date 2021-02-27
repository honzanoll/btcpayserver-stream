using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Business.Converters;
using BTCPayServer.Stream.Business.Converters.Abstractions;
using BTCPayServer.Stream.Business.Extensions;
using BTCPayServer.Stream.Business.Models.Streamlabs;
using BTCPayServer.Stream.Business.Services.Abstractions;
using BTCPayServer.Stream.Common.Extensions;
using BTCPayServer.Stream.Common.Models.Settings;
using BTCPayServer.Stream.Common.Models.Settings.HttpClients;
using BTCPayServer.Stream.Data.Enums;
using BTCPayServer.Stream.Data.Models.OAuth;
using BTCPayServer.Stream.HttpClients.Abstractions;
using BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Requests;
using BTCPayServer.Stream.HttpClients.StreamlabsClient.Models.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using honzanoll.Repository.Abstractions;
using System;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Business.Services
{
    public class StreamlabsService : IStreamlabsService
    {
        #region Fields

        private readonly IPriceStore priceStore;

        private readonly IConverter<EmojiConverter> emojiConverter;

        private readonly IStreamlabsHttpClient streamlabsHttpClient;

        private readonly IRepository<StreamlabsAuthToken> streamlabsAuthTokenRepository;

        private readonly GlobalSettings globalSettings;
        private readonly StreamlabsSettings streamlabsSettings;

        private ILogger<StreamlabsService> logger;

        #endregion

        #region Constructors

        public StreamlabsService(
            IPriceStore priceStore,
            IConverter<EmojiConverter> emojiConverter,
            IStreamlabsHttpClient streamlabsHttpClient,
            IRepository<StreamlabsAuthToken> streamlabsAuthTokenRepository,
            IOptions<GlobalSettings> globalSettingsOptions,
            IOptions<StreamlabsSettings> streamlabsSettingsOptions,
            ILogger<StreamlabsService> logger)
        {
            this.priceStore = priceStore;

            this.emojiConverter = emojiConverter;

            this.streamlabsHttpClient = streamlabsHttpClient;

            this.streamlabsAuthTokenRepository = streamlabsAuthTokenRepository;

            globalSettings = globalSettingsOptions.Value;
            streamlabsSettings = streamlabsSettingsOptions.Value;

            this.logger = logger;
        }

        #endregion

        #region Public methods

        public async Task ObtainAccessTokenAsync(string code, Guid userId)
        {
            GetAccessTokenResponse response = await streamlabsHttpClient.GetAccessTokenAsync(new GetAccessTokenRequest
            {
                ClientId = streamlabsSettings.ClientId,
                ClientSecret = streamlabsSettings.ClientSecret,
                RedirectUri = globalSettings.Infrastructure.FEUrl + "/oauth/streamlabs/callback",
                Code = code
            });

            await streamlabsAuthTokenRepository.CreateAsync(new StreamlabsAuthToken
            {
                Token = response.AccessToken,
                UserId = userId,
                Created = DateTime.Now,
                CreatedBy = "SYSTEM"
            });
        }

        public async Task ClearTokenAsync(Guid userId)
        {
            StreamlabsAuthToken streamlabsAuthToken = await streamlabsAuthTokenRepository.GetAsync(sat => sat.UserId == userId);
            if (streamlabsAuthToken == null)
                return;

            await streamlabsAuthTokenRepository.DeleteAsync(streamlabsAuthToken);
        }

        public async Task SendDonateAsync(Guid userId, Donate donate)
        {
            string accessToken = await GetAccessTokenAsync(userId);
            if (accessToken == null)
                return;

            double amount = donate.Amount;
            if (donate.Currency == InvoiceCurrency.SAT)
                amount = amount / 100000000 * priceStore.GetPrice(Currency.USD);

            SendDonateRequest request = new SendDonateRequest
            {
                Name = donate.Name,
                Message = emojiConverter.Convert(donate.Message),
                Identifier = donate.Identifier,
                Amount = amount,
                Currency = donate.Currency.ToISO().ToString(),
                AccessToken = accessToken
            };

            await streamlabsHttpClient.SendDonateAsync(request);

            logger.LogInformation($"Donate has been successfully sent (data: {request.ToJson()})");
        }

        public string PrepareMessage(string message)
        {
            if (message == null)
                return null;

            string finalMessage = $" {message} "
                .Replace(Environment.NewLine, " ")
                .Replace("\r\n", " ")
                .Replace("\n", " ");

            finalMessage = emojiConverter
                .Convert(finalMessage)
                .Trim();

            return finalMessage;
        }

        #endregion

        #region Private methods

        public async Task<string> GetAccessTokenAsync(Guid userId)
        {
            StreamlabsAuthToken streamlabsAuthToken = await streamlabsAuthTokenRepository.GetAsync(sat => sat.UserId == userId);
            if (streamlabsAuthToken == null)
                return null;

            return streamlabsAuthToken.Token;
        }

        #endregion
    }
}
